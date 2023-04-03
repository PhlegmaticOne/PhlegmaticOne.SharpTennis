using PhlegmaticOne.SharpTennis.Game.Common.Commands;
using PhlegmaticOne.SharpTennis.Game.Engine2D;
using PhlegmaticOne.SharpTennis.Game.Game.Scenes.Base;

namespace PhlegmaticOne.SharpTennis.Game.Game.Commands
{
    internal class StartGameCommand : ICommand
    {
        private readonly CanvasManager _canvasManager;
        private readonly ISceneBuilder _sceneBuilder;
        private readonly GameRunner _gameRunner;

        public StartGameCommand(CanvasManager canvasManager, 
            ISceneBuilder sceneBuilder,
            GameRunner gameRunner)
        {
            _canvasManager = canvasManager;
            _sceneBuilder = sceneBuilder;
            _gameRunner = gameRunner;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            _canvasManager.RemoveLast(true);
            _sceneBuilder.BuildScene();
            _gameRunner.RenderFormResizedCallback(null, null);
        }
    }
}
