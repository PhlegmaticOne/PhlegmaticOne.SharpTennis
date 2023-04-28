using PhlegmaticOne.SharpTennis.Game.Common.Base.Scenes;
using PhlegmaticOne.SharpTennis.Game.Common.Commands;
using PhlegmaticOne.SharpTennis.Game.Engine2D;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Game;
using PhlegmaticOne.SharpTennis.Game.Game.Scenes.Base;

namespace PhlegmaticOne.SharpTennis.Game.Game.Commands
{
    internal class StartGameCommand : ICommand
    {
        private readonly CanvasManager _canvasManager;
        private readonly SceneProvider _sceneProvider;
        private readonly GameCanvasFactory _gameRunnerFactory;
        private readonly ISceneBuilderFactory<TennisGameScenes> _sceneBuilder;
        private readonly GameRunner<TennisGameScenes> _gameRunner;

        public StartGameCommand(CanvasManager canvasManager, 
            SceneProvider sceneProvider,
            GameCanvasFactory gameRunnerFactory,
            ISceneBuilderFactory<TennisGameScenes> sceneBuilder,
            GameRunner<TennisGameScenes> gameRunner)
        {
            _canvasManager = canvasManager;
            _sceneProvider = sceneProvider;
            _gameRunnerFactory = gameRunnerFactory;
            _sceneBuilder = sceneBuilder;
            _gameRunner = gameRunner;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            _canvasManager.RemoveLast();
            var sceneBuilder = _sceneBuilder.CreateSceneBuilder(_sceneBuilder.Scenes.Game);
            var scene = sceneBuilder.BuildScene();
            var canvas = _gameRunnerFactory.CreateCanvasForScene(scene);
            _canvasManager.AddCanvas(canvas, false);
            _sceneProvider.ChangeScene(scene);
            _gameRunner.ForceResize();
        }
    }
}
