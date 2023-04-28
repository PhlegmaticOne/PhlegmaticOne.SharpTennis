using System.Collections.Generic;
using PhlegmaticOne.SharpTennis.Game.Common.Tween;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements
{
    public enum GameState
    {
        None,
        Knock
    }

    public class GameStateViewController : RectComponent
    {
        private TextComponent _textComponent;

        private readonly Dictionary<GameState, string> _gameStates;
        private GameState _currentState;

        public GameStateViewController()
        {
            _gameStates = new Dictionary<GameState, string>();
            InitializeGameStates();
        }

        public void SetTextComponent(TextComponent textComponent)
        {
            _textComponent = textComponent;
            RectTransform = textComponent.RectTransform;
        }

        public void Show(GameState gameState, string parameter)
        {
            if (_currentState != GameState.None)
            {
                _textComponent.RectTransform.DoKill();
            }

            _currentState = gameState;
            var value = _gameStates[gameState];
            var result = string.Format(value, parameter);
            _textComponent.Text = result;
            _textComponent.RectTransform.DoScale(Vector3.Zero, Vector3.One, 1f, false, () =>
            {
                _currentState = GameState.None;
            });
        }

        private void InitializeGameStates()
        {
            _gameStates.Add(GameState.Knock, "{0} knock");
        }
    }
}
