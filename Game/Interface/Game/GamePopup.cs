using System;
using PhlegmaticOne.SharpTennis.Game.Common.Infrastructure;
using PhlegmaticOne.SharpTennis.Game.Common.Sound.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using PhlegmaticOne.SharpTennis.Game.Game.Controllers;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Game
{
    public class GamePopup : GamePopupBase
    {
        private float _remainTime;
        private bool _updateTimer;

        private readonly BallBouncesController _ballBouncesController;
        private readonly CanvasManager _canvasManager;
        private ScoreSystem _scoreSystem;
        private GameStateViewController _gameStateViewController;
        private TextComponent _infoText;
        private TextComponent _fpsText;
        private TextComponent _timeText;

        public GamePopup(BallBouncesController ballBouncesController, CanvasManager canvasManager,
            ISoundManager<GameSounds> soundManager) : base(soundManager)
        {
            _ballBouncesController = ballBouncesController;
            _canvasManager = canvasManager;
            Time.Updated += TimeOnUpdated;
            _ballBouncesController.Losed += BallBouncesControllerOnLosed;
            _ballBouncesController.StateChanged += BallBouncesControllerOnStateChanged;
            _ballBouncesController.Restarted += BallBouncesControllerOnRestarted;
        }

        private void TimeOnUpdated()
        {
            _fpsText.Text = "FPS: " + Time.Fps;

            if (_updateTimer)
            {
                _remainTime -= Time.DeltaT;
                _timeText.Text = TimeSpan.FromSeconds(_remainTime).ToString("g");

                if (_remainTime <= 0)
                {
                    _timeText.Text = string.Empty;
                    _timeText.Enabled = false;
                    _updateTimer = false;
                }
            }
        }

        private void BallBouncesControllerOnRestarted()
        {
            _scoreSystem.EnemyText.ResetScore();
            _scoreSystem.PlayerText.ResetScore();
        }

        public void SetupViews(ScoreSystem scoreSystem, GameStateViewController gameStateViewController,
            TextComponent infoText, TextComponent fpsText, TextComponent timeText)
        {
            _scoreSystem = scoreSystem;
            _gameStateViewController = gameStateViewController;
            _fpsText = fpsText;
            _timeText = timeText;
            _infoText = infoText;
        }

        public void SetupGameData(GameData gameData)
        {
            var text = $"Game to score: {gameData.PlayToScore}\nDifficulty: {gameData.DifficultyType}";
            _infoText.Text = text;

            if (gameData.GameType == GameType.Time)
            {
                _remainTime = gameData.TimeInMinutes * 60;
                _updateTimer = true;
            }
        }

        protected override void OnShow()
        {
            _canvasManager.ChangeCursorEnabled(false);
        }

        protected override void OnClose()
        {
            _updateTimer = false;
            base.OnClose();
        }


        private void BallBouncesControllerOnStateChanged(GameState obj, RacketType parameter)
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
