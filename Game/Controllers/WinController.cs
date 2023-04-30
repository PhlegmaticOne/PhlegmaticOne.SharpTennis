using System.Collections.Generic;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Win;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;

namespace PhlegmaticOne.SharpTennis.Game.Game.Controllers
{
    public class WinController : BehaviorObject
    {
        private const int WinScore = 1;
        private readonly PopupSystem _popupSystem;

        private Dictionary<RacketType, int> _loses;
        private BallBouncesController _ballBouncesController;
        private RacketMoveController _moveController;


        public WinController(PopupSystem popupSystem)
        {
            _popupSystem = popupSystem;
            _loses = InitLoses();
        }

        public void Setup(BallBouncesController ballBouncesController,
            RacketMoveController racketMoveController)
        {
            _ballBouncesController = ballBouncesController;
            _moveController = racketMoveController;
            _ballBouncesController.Losed += BallBouncesControllerOnLosed;
        }

        public override void OnDestroy()
        {
            _loses = InitLoses();
        }

        private Dictionary<RacketType, int> InitLoses() =>
            new Dictionary<RacketType, int>
            {
                { RacketType.Enemy, 0 },
                { RacketType.Player, 0 },
            };


        private void BallBouncesControllerOnLosed(RacketType obj)
        {
            _loses[obj]++;

            if (_loses[obj] == WinScore)
            {
                _moveController.ChangeEnabled(false);
                var winner = obj == RacketType.Player ? RacketType.Enemy : RacketType.Player;
                SpawnWinPopup(winner);
            }
        }

        private void SpawnWinPopup(RacketType winner)
        {
            var winPopup = _popupSystem.SpawnPopup<WinPopup>();
            winPopup.SetWinner(winner);
        }
    }
}
