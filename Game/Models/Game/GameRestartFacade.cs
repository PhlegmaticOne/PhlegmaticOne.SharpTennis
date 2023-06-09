﻿using System.Linq;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Game.Controllers;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Game;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Racket;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Game
{
    public class GameRestartFacade : IGameRestartFacade
    {
        private readonly TennisGameController _ballBouncesController;
        private readonly GameDataProvider _gameDataProvider;
        private readonly WinController _winController;
        private readonly PopupSystem _popupSystem;
        private BallModel _ballModel;
        private PlayerRacket _playerRacket;
        private EnemyRacket _enemyRacket;

        public GameRestartFacade(TennisGameController ballBouncesController,
            GameDataProvider gameDataProvider,
            WinController winController,
            PopupSystem popupSystem)
        {
            _ballBouncesController = ballBouncesController;
            _gameDataProvider = gameDataProvider;
            _winController = winController;
            _popupSystem = popupSystem;
        }

        public void Setup(BallModel ballModel, PlayerRacket playerRacket, EnemyRacket enemyRacket)
        {
            _ballModel = ballModel;
            _playerRacket = playerRacket;
            _enemyRacket = enemyRacket;
        }

        public void Restart(bool isFromWinning)
        {
            if (isFromWinning)
            {
                ApplyGameData();
            }

            _ballModel.Reset();
            _playerRacket.Reset();
            _enemyRacket.Reset();
            _ballBouncesController.Restart();
        }

        private void ApplyGameData()
        {
            var gameData = _gameDataProvider.GameData;
            var difficulty = gameData.DifficultyType;
            _enemyRacket.SetupDifficulty(difficulty);
            _playerRacket.SetupDifficulty(difficulty);
            _winController.SetupGameData(gameData);
            TrySetupGamePopup();
            ChangeRacketColors(gameData);
        }

        private void TrySetupGamePopup()
        {
            if (_popupSystem.Popups.First() is GamePopup first)
            {
                first.SetupGameData(_gameDataProvider.GameData);
            }
        }

        private void ChangeRacketColors(GameData gameData)
        {
            if(gameData.PlayerColor == ColorType.Custom)
            {
                _playerRacket.Color(gameData.ParsedColor());
                _enemyRacket.Color(ColorType.Black);
                return;
            }
            var playerColor = gameData.PlayerColor == ColorType.Red ? ColorType.Red : ColorType.Black;
            var enemyColor = gameData.PlayerColor == ColorType.Red ? ColorType.Black : ColorType.Red;
            _playerRacket.Color(playerColor);
            _enemyRacket.Color(enemyColor);
        }
    }
}
