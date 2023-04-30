using PhlegmaticOne.SharpTennis.Game.Common.Base.Scenes;
using PhlegmaticOne.SharpTennis.Game.Common.Commands;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Game;
using PhlegmaticOne.SharpTennis.Game.Game.Scenes.Base;

namespace PhlegmaticOne.SharpTennis.Game.Game.Commands
{
    internal class StartGameCommand : ICommand
    {
        private readonly PopupSystem _popupSystem;
        private readonly SceneProvider _sceneProvider;
        private readonly ISceneBuilderFactory<TennisGameScenes> _sceneBuilder;
        private readonly GameRunner<TennisGameScenes> _gameRunner;

        public StartGameCommand(PopupSystem popupSystem, 
            SceneProvider sceneProvider,
            ISceneBuilderFactory<TennisGameScenes> sceneBuilder,
            GameRunner<TennisGameScenes> gameRunner)
        {
            _popupSystem = popupSystem;
            _sceneProvider = sceneProvider;
            _sceneBuilder = sceneBuilder;
            _gameRunner = gameRunner;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            _popupSystem.CloseAll();
            var sceneBuilder = _sceneBuilder.CreateSceneBuilder(_sceneBuilder.Scenes.Game);
            var scene = sceneBuilder.BuildScene();
            _sceneProvider.ChangeScene(scene);
            var popup = _popupSystem.SpawnPopup<GamePopup>();
            _gameRunner.ForceResize();
        }
    }
}
