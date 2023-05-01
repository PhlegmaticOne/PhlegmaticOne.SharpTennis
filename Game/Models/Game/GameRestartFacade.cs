using PhlegmaticOne.SharpTennis.Game.Game.Controllers;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Racket;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Game
{
    public class GameRestartFacade : IGameRestartFacade
    {
        private readonly BallBouncesController _ballBouncesController;
        private BallModel _ballModel;
        private PlayerRacket _playerRacket;
        private EnemyRacket _enemyRacket;

        public GameRestartFacade(BallBouncesController ballBouncesController) => 
            _ballBouncesController = ballBouncesController;

        public void Setup(BallModel ballModel, PlayerRacket playerRacket, EnemyRacket enemyRacket)
        {
            _ballModel = ballModel;
            _playerRacket = playerRacket;
            _enemyRacket = enemyRacket;
        }

        public void Restart()
        {
            _ballModel.Reset();
            _playerRacket.Reset();
            _enemyRacket.Reset();
            _ballBouncesController.Restart();
        }
    }
}
