using PhlegmaticOne.SharpTennis.Game.Common.Infrastructure;
using PhlegmaticOne.SharpTennis.Game.Game.Controllers;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Racket;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Game
{
    public class GamePauseFacade : IGamePauseFacade
    {
        private BallModel _ballModel;
        private RacketMoveController _racketMoveController;
        private EnemyRacket _enemyRacket;

        public void Setup(BallModel ball, RacketMoveController racketMoveController, EnemyRacket enemyRacket)
        {
            _ballModel = ball;
            _racketMoveController = racketMoveController;
            _enemyRacket = enemyRacket;
        }

        public void Pause()
        {
            Time.Paused = true;
            _ballModel.ChangeActive(false);
            _enemyRacket.ChangeActive(false);
            _racketMoveController.ChangeEnabled(false);
        }

        public void Continue()
        {
            Time.Paused = false;
            _ballModel.ChangeActive(true);
            _enemyRacket.ChangeActive(true);
            _racketMoveController.ChangeEnabled(true);
        }
    }
}
