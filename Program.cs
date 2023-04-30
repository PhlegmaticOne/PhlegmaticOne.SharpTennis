using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using PhlegmaticOne.SharpTennis.Game.Common.Base.Scenes;
using PhlegmaticOne.SharpTennis.Game.Common.Input;
using PhlegmaticOne.SharpTennis.Game.Common.Render;
using PhlegmaticOne.SharpTennis.Game.Engine2D;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.DirectX;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Engine3D.DirectX;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Game;
using PhlegmaticOne.SharpTennis.Game.Game.Commands;
using PhlegmaticOne.SharpTennis.Game.Game.Controllers;
using PhlegmaticOne.SharpTennis.Game.Game.Infrastructure;
using PhlegmaticOne.SharpTennis.Game.Game.Interface;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Game;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Menu;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Win;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Floor;
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
            serviceCollection.AddSingleton(x =>
            {
                var exit = x.GetRequiredService<ExitGameCommand>();
                var start = x.GetRequiredService<StartGameCommand>();
                return new MenuCanvasViewModel(start, exit);
            });
            serviceCollection.AddSingleton<MenuSceneBuilder>();
            serviceCollection.AddSingleton(x =>
            {
                var canvasManager = x.GetRequiredService<CanvasManager>();
                return new PopupSystem(x, canvasManager);
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

            AddPopup<WinPopup, WinPopupFactory>(serviceCollection);
            AddPopup<MenuPopup, MenuPopupFactory>(serviceCollection);
            AddPopup<GamePopup, GamePopupFactory>(serviceCollection);
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

            serviceCollection.AddSingleton<GameRunner<TennisGameScenes>>();
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
