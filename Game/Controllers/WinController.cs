using System.Collections.Generic;
using System.Linq;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Infrastructure;
using PhlegmaticOne.SharpTennis.Game.Common.Sound.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Win;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game.Base;

namespace PhlegmaticOne.SharpTennis.Game.Game.Controllers
{
    public class WinController : BehaviorObject
    {
        private int _winScore;
        private int _playTime;
        private float _remainTime;
        private GameData _gameData;
        private readonly PopupSystem _popupSystem;
        private readonly ISoundManager<GameSounds> _soundManager;
        private readonly IGamePauseFacade _gamePauseFacade;

        private Dictionary<RacketType, int> _loses;
        private BallBouncesController _ballBouncesController;

        public WinController(PopupSystem popupSystem, ISoundManager<GameSounds> soundManager,
            IGamePauseFacade gamePauseFacade)
        {
            _popupSystem = popupSystem;
            _soundManager = soundManager;
            _gamePauseFacade = gamePauseFacade;
            _loses = InitLoses();
        }

        public void Setup(BallBouncesController ballBouncesController)
        {
            _ballBouncesController = ballBouncesController;
            _ballBouncesController.Losed += BallBouncesControllerOnLosed;
            _ballBouncesController.Restarted += BallBouncesControllerOnRestarted;
        }

        private void BallBouncesControllerOnRestarted()
        {
            _loses = InitLoses();
        }

        public void SetupGameData(GameData gameData)
        {
            _winScore = gameData.PlayToScore;
            _playTime = gameData.TimeInMinutes * 60;
            _gameData = gameData;

            if (_gameData.GameType == GameType.Time)
            {
                _remainTime = _playTime;
                Time.Updated += TimeOnUpdated;
            }
        }

        private void TimeOnUpdated()
        {
            _remainTime -= Time.DeltaT;

            if (_remainTime <= 0)
            {
                Time.Updated -= TimeOnUpdated;
                var winner = _loses[RacketType.Player] < _loses[RacketType.Enemy] ? RacketType.Player : RacketType.Enemy;
                if (_loses[RacketType.Player] == _loses[RacketType.Enemy])
                {
                    winner = RacketType.None;
                }

                _gamePauseFacade.Pause();
                PlaySound(winner);
                SpawnWinPopup(winner);
            }
        }

        public override void OnDestroy()
        {
            _loses = InitLoses();
            Time.Updated -= TimeOnUpdated;
            _ballBouncesController.Losed -= BallBouncesControllerOnLosed;
            _ballBouncesController.Restarted -= BallBouncesControllerOnRestarted;
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
            var winner = obj == RacketType.Player ? RacketType.Enemy : RacketType.Player;
            PlaySound(winner);

            if (_gameData.GameType == GameType.Time)
            {
                return;
            }

            if (_loses[obj] == _winScore)
            {
                _gamePauseFacade.Pause();
                SpawnWinPopup(winner);
            }
        }

        private void PlaySound(RacketType winner)
        {
            var sound = winner == RacketType.Player ? GameSounds.Win : GameSounds.Lose;
            _soundManager.Play(sound);
        }

        private void SpawnWinPopup(RacketType winner)
        {
            var winPopup = _popupSystem.SpawnPopup<WinPopup>();
            winPopup.SetWinner(winner);
        }
    }
}
