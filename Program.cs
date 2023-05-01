using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using PhlegmaticOne.SharpTennis.Game.Common.Base.Scenes;
using PhlegmaticOne.SharpTennis.Game.Common.Extensions;
using PhlegmaticOne.SharpTennis.Game.Common.Input;
using PhlegmaticOne.SharpTennis.Game.Common.Render;
using PhlegmaticOne.SharpTennis.Game.Common.Sound;
using PhlegmaticOne.SharpTennis.Game.Common.Sound.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Sound.Models.Data;
using PhlegmaticOne.SharpTennis.Game.Engine2D;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.DirectX;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Engine3D.DirectX;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Game;
using PhlegmaticOne.SharpTennis.Game.Game.Commands;
using PhlegmaticOne.SharpTennis.Game.Game.Commands.Global;
using PhlegmaticOne.SharpTennis.Game.Game.Controllers;
using PhlegmaticOne.SharpTennis.Game.Game.Global;
using PhlegmaticOne.SharpTennis.Game.Game.Infrastructure;
using PhlegmaticOne.SharpTennis.Game.Game.Interface;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Game;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.GameSettings;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Menu;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Pause;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Settings;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Win;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Floor;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Racket;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Sky;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Table;
using PhlegmaticOne.SharpTennis.Game.Game.Scenes;
using PhlegmaticOne.SharpTennis.Game.Game.Scenes.Base;
using SharpDX.Direct3D;
using SharpDX.Windows;
using Device11 = SharpDX.Direct3D11.Device;

namespace PhlegmaticOne.SharpTennis.Game
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            if(TryStartGame() == false)
            {
                return;
            }

            StartGame();
        }

        private static void StartGame()
        {
            var serviceProvider = RegisterServices();
            var gameRunner = serviceProvider.GetRequiredService<GameRunner<TennisGameScenes>>();

            using (gameRunner)
            {
                gameRunner.Run();
            }
        }

        private static IServiceProvider RegisterServices()
        {
            var serviceCollection = new ServiceCollection();
            RegisterInfrastructure(serviceCollection);
            RegisterGameServices(serviceCollection);
            return serviceCollection.BuildServiceProvider();
        }

        private static void RegisterGameServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ExitGameCommand>();
            serviceCollection.AddSingleton<StartGameCommand>();
            serviceCollection.AddSingleton<CloseLastPopupCommand>();
            serviceCollection.AddSingleton<SaveSoundSettingsCommand>();
            serviceCollection.AddSingleton<PauseGameCommand>();
            serviceCollection.AddSingleton<ContinueGameCommand>();
            serviceCollection.AddSingleton<ExitToMenuCommand>();
            serviceCollection.AddSingleton<RestartGameCommand>();
            serviceCollection.AddSingleton(x =>
            {
                var exit = x.GetRequiredService<ExitGameCommand>();
                var popupSystem = x.GetRequiredService<PopupSystem>();
                var spawnGameSettings = new SpawnPopupCommand<GameSettingsPopup>(popupSystem);
                var spawnSettings = new SpawnPopupCommand<SettingsPopup>(popupSystem);
                return new MenuPopupViewModel(spawnGameSettings, exit, spawnSettings);
            });
            serviceCollection.AddSingleton(x =>
            {
                var close = x.GetRequiredService<CloseLastPopupCommand>();
                var start = x.GetRequiredService<StartGameCommand>();
                return new GameSettingsViewModel(close, start);
            });
            serviceCollection.AddSingleton(x =>
            {
                var close = x.GetRequiredService<CloseLastPopupCommand>();
                var save = x.GetRequiredService<SaveSoundSettingsCommand>();
                return new SettingsPopupViewModel(close, save);
            });
            serviceCollection.AddSingleton(x =>
            {
                var continueCommand = x.GetRequiredService<RestartGameCommand>();
                var exitToMenu = x.GetRequiredService<ExitToMenuCommand>();
                return new WinPopupViewModel(continueCommand, exitToMenu);
            });
            serviceCollection.AddSingleton(x =>
            {
                var popup = x.GetRequiredService<PopupSystem>();
                var onShow = x.GetRequiredService<PauseGameCommand>();
                var continueCommand = x.GetRequiredService<ContinueGameCommand>();
                var settingsCommand = new SpawnPopupCommand<SettingsPopup>(popup);
                var exitCommand = x.GetRequiredService<ExitToMenuCommand>();
                var restart = x.GetRequiredService<RestartGameCommand>();
                return new PausePopupViewModel(onShow, continueCommand, exitCommand, settingsCommand, restart);
            });

            serviceCollection.AddSingleton<MenuSceneBuilder>();
            serviceCollection.AddSingleton(x =>
            {
                var canvasManager = x.GetRequiredService<CanvasManager>();
                return new PopupSystem(x, canvasManager);
            });

            serviceCollection.AddSingleton<ISoundManager<GameSounds>, SoundManager<GameSounds>>(x =>
            {
                var factory = x.GetRequiredService<SharpAudioVoiceFactory>();
                return new SoundManager<GameSounds>(new Dictionary<GameSounds, SharpAudioVoice>
                {
                    { GameSounds.PopupIn, factory.CreateVoice(@"assets\sounds\popup_in.wav") },
                    { GameSounds.PopupOut, factory.CreateVoice(@"assets\sounds\popup_out.wav") },
                    { GameSounds.TableBounce, factory.CreateVoice(@"assets\sounds\table_bounce.wav") },
                    { GameSounds.RacketBounce, factory.CreateVoice(@"assets\sounds\racket_bounce.wav") },
                    { GameSounds.FloorBounce, factory.CreateVoice(@"assets\sounds\floor_bounce.wav") },
                    { GameSounds.Lose, factory.CreateVoice(@"assets\sounds\lose.wav") },
                    { GameSounds.Win, factory.CreateVoice(@"assets\sounds\win.wav") },
                });
            });

            serviceCollection.AddSingleton<GameSceneBuilder>();

            serviceCollection.AddSingleton<ISceneBuilderFactory<TennisGameScenes>, SceneBuilderFactory>();

            serviceCollection.AddSingleton<IFactory<TennisTable>, TennisTableFactory>();
            serviceCollection.AddSingleton<IFactory<RacketBase, RacketFactoryData>, RacketFactory>();
            serviceCollection.AddSingleton<IFactory<BallModel>, BallFactory>();
            serviceCollection.AddSingleton<IFactory<FloorModel>, FloorFactory>();
            serviceCollection.AddSingleton<IFactory<SkyModel>, SkyFactory>();
            serviceCollection.AddSingleton<BallBounceProvider>();
            serviceCollection.AddSingleton<WinController>();
            serviceCollection.AddSingleton<BallBouncesController>();

            serviceCollection.AddTransient<InputNumberSelectableElement>();
            serviceCollection.AddSingleton<GameDataProvider>();
            serviceCollection.AddSingleton<ISoundSettingsProvider, JsonSoundSettingsProvider>();

            serviceCollection.AddSingleton<MenuEscapeButtonHandler>();
            serviceCollection.AddSingleton<GameEscapeButtonHandler>();
            serviceCollection.AddSingleton<MenuListenerInitializer>();
            serviceCollection.AddSingleton<GameListenerInitializer>();

            serviceCollection.AddSingleton<IGamePauseFacade>(x => x.GetRequiredService<GamePauseFacade>());
            serviceCollection.AddSingleton<IGameRestartFacade>(x => x.GetRequiredService<GameRestartFacade>());
            serviceCollection.AddSingleton<GamePauseFacade>();
            serviceCollection.AddSingleton<GameRestartFacade>();

            AddPopup<WinPopup, WinPopupFactory>(serviceCollection);
            AddPopup<MenuPopup, MenuPopupFactory>(serviceCollection);
            AddPopup<GamePopup, GamePopupFactory>(serviceCollection);
            AddPopup<GameSettingsPopup, GameSettingPopupFactory>(serviceCollection);
            AddPopup<SettingsPopup, SettingsPopupFactory>(serviceCollection);
            AddPopup<PausePopup, PausePopupFactory>(serviceCollection);
        }

        private static void AddPopup<TPopup, TFactory>(IServiceCollection serviceCollection) 
            where TPopup : PopupBase
            where TFactory : PopupFactory<TPopup>
        {
            serviceCollection.AddTransient<TPopup>();
            serviceCollection.AddSingleton<PopupFactory<TPopup>, TFactory>();
        }

        private static void RegisterInfrastructure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(new RenderForm
            {
                IsFullscreen = true,
                MinimumSize = new Size(960, 540),
                WindowState = FormWindowState.Maximized
            });

            serviceCollection.AddSingleton<DirectX3DGraphics>();
            serviceCollection.AddSingleton<DirectX2DGraphics>();
            serviceCollection.AddSingleton<IElementsCreator>(x => x.GetRequiredService<DirectX2DGraphics>());
            serviceCollection.AddSingleton<IElementsRenderer>(x => x.GetRequiredService<DirectX2DGraphics>());
            serviceCollection.AddSingleton<InputController>();
            serviceCollection.AddSingleton<MeshLoader>();
            serviceCollection.AddSingleton(x =>
                new TextureProviderFactory(x.GetRequiredService<MeshLoader>()).CreateTextureProvider());
            serviceCollection.AddSingleton(x =>
            {
                var graphics = x.GetRequiredService<DirectX3DGraphics>();
                var sceneProvider = x.GetRequiredService<SceneProvider>();
                return new MeshRenderer(graphics, new MeshRendererData(
                    new ShaderInfo("vertex.hlsl", "vertexShader", "vs_5_0"),
                    new ShaderInfo("pixel.hlsl", "pixelShader", "ps_5_0")), sceneProvider);
            });
            serviceCollection.AddSingleton<CanvasManagerFactory>();
            serviceCollection.AddSingleton(x => x.GetRequiredService<CanvasManagerFactory>().CreateCanvasManager());
            serviceCollection.AddSingleton<IRenderer>(x => x.GetRequiredService<CanvasManager>());
            serviceCollection.AddSingleton<IRenderer>(x => x.GetRequiredService<MeshRenderer>());
            serviceCollection.AddSingleton(x =>
            {
                var meshRenderer = x.GetRequiredService<MeshRenderer>();
                var canvasManager = x.GetRequiredService<CanvasManager>();
                return new RenderSequence(new IRenderer[] { meshRenderer, canvasManager });
            });
            serviceCollection.AddSingleton(x => x.GetRequiredService<MeshRenderer>().PointSampler);
            serviceCollection.AddSingleton<SceneProvider>();
            serviceCollection.AddSingleton<SharpAudioDevice>();
            serviceCollection.AddSingleton<SharpAudioVoiceFactory>();

            serviceCollection.AddSingleton<GameRunner<TennisGameScenes>>();
            serviceCollection.AddSingleton(x =>
            {
                var input = x.GetRequiredService<InputController>();
                return new GlobalInputListener(input).WrapWithGameObject();
            });
        }

        private static bool TryStartGame()
        {
            if (Device11.GetSupportedFeatureLevel() != FeatureLevel.Level_11_0)
            {
                MessageBox.Show("DirectX11 not Supported");
                return false;
            }

            return true;
        }
    }
}
