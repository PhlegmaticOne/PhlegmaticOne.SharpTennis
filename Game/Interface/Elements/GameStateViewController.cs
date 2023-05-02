using System.Collections.Generic;
using PhlegmaticOne.SharpTennis.Game.Common.Tween;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game.Player.Data;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements
{
    public enum GameState
    {
        None,
        Knock,
        DidntKnock,
        DidntHitTable,
        DidntHitOppositeTable,
        TooManyBouncesFromTable,
        KnockSucceed,
        Kicked,
        KickSucceed
    }

    public class GameStateViewController : RectComponent
    {
        private readonly IPlayerDataProvider _playerDataProvider;
        private TextComponent _textComponent;

        private readonly Dictionary<GameState, string> _gameStates;
        private GameState _currentState;

        public GameStateViewController(IPlayerDataProvider playerDataProvider)
        {
            _playerDataProvider = playerDataProvider;
            _gameStates = new Dictionary<GameState, string>();
            InitializeGameStates();
        }

        public void SetTextComponent(TextComponent textComponent)
        {
            _textComponent = textComponent;
            RectTransform = textComponent.RectTransform;
        }

        public void Show(GameState gameState, RacketType racketType)
        {
            if (_currentState != GameState.None)
            {
                _textComponent.RectTransform.DoKill();
            }

            _currentState = gameState;
            var value = _gameStates[gameState];
            var result = string.Format(value, GetParameter(racketType));
            _textComponent.Text = result;
            _textComponent.RectTransform.DoScale(Vector3.Zero, Vector3.One, 0.4f, false, () =>
            {
                _currentState = GameState.None;
            });
        }

        private string GetParameter(RacketType racketType)
        {
            if (racketType == RacketType.Player)
            {
                return _playerDataProvider.PlayerData.Name;
            }

            if (racketType == RacketType.Enemy)
            {
                return RacketType.Enemy.ToString();
            }

            return string.Empty;
        }

        private void InitializeGameStates()
        {
            _gameStates.Add(GameState.Knock, "{0} knocked");
            _gameStates.Add(GameState.DidntKnock, "{0} didn't knock");
            _gameStates.Add(GameState.DidntHitTable, "{0} didn't hit table");
            _gameStates.Add(GameState.DidntHitOppositeTable, "{0} didn't hit opposite table");
            _gameStates.Add(GameState.TooManyBouncesFromTable, "Too many bounces from table: {0}");
            _gameStates.Add(GameState.KnockSucceed, "{0} succeeds his knock");
            _gameStates.Add(GameState.Kicked, "{0} kicked");
            _gameStates.Add(GameState.KickSucceed, "{0} succeeds his kick");
        }
    }
}
