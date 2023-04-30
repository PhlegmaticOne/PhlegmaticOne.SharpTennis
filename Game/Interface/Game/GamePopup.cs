using PhlegmaticOne.SharpTennis.Game.Engine2D;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Game.Controllers;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Game
{
    public class GamePopup : PopupBase
    {
        private readonly BallBouncesController _ballBouncesController;
        private readonly CanvasManager _canvasManager;
        private ScoreSystem _scoreSystem;
        private GameStateViewController _gameStateViewController;

        public GamePopup(BallBouncesController ballBouncesController, CanvasManager canvasManager)
        {
            _ballBouncesController = ballBouncesController;
            _canvasManager = canvasManager;
            _ballBouncesController.Losed += BallBouncesControllerOnLosed;
            _ballBouncesController.StateChanged += BallBouncesControllerOnStateChanged;
        }

        public void Setup(ScoreSystem scoreSystem, GameStateViewController gameStateViewController)
        {
            _scoreSystem = scoreSystem;
            _gameStateViewController = gameStateViewController;
        }

        protected override void OnShow()
        {
            _canvasManager.ChangeCursorEnabled(false);
        }


        private void BallBouncesControllerOnStateChanged(GameState obj, string parameter)
        {
            _gameStateViewController.Show(obj, parameter);
        }

        private void BallBouncesControllerOnLosed(RacketType obj)
        {
            var addScore = obj == RacketType.Player ? RacketType.Enemy : RacketType.Player;
            _scoreSystem.AddScore(1, addScore);
        }
    }
}
