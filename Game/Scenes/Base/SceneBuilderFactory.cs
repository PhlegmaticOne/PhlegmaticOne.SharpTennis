using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using PhlegmaticOne.SharpTennis.Game.Common.Base.Scenes;

namespace PhlegmaticOne.SharpTennis.Game.Game.Scenes.Base
{
    public class TennisGameScenes : IGameScenes
    {
        public TennisGameScenes()
        {
            Default = new SceneType("Default");
            Menu = new SceneType("Menu");
            ChooseGameMode = new SceneType("ChooseGameMode");
            Game = new SceneType("Game");
        }

        public SceneType Default { get; }
        public SceneType Menu { get; }
        public SceneType ChooseGameMode { get; }
        public SceneType Game { get; }
    }

    public class SceneBuilderFactory : ISceneBuilderFactory<TennisGameScenes>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<SceneType, Func<ISceneBuilder>> _sceneBuilders;

        public SceneBuilderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Scenes = new TennisGameScenes();

            _sceneBuilders = new Dictionary<SceneType, Func<ISceneBuilder>>
            {
                { Scenes.Default, () => _serviceProvider.GetRequiredService<MenuSceneBuilder>() },
                { Scenes.Menu, () => _serviceProvider.GetRequiredService<MenuSceneBuilder>() },
                { Scenes.Game, () => _serviceProvider.GetRequiredService<GameSceneBuilder>() }
            };
        }

        public TennisGameScenes Scenes { get; }

        public ISceneBuilder CreateSceneBuilder(SceneType sceneType) => _sceneBuilders[sceneType]();
    }
}
