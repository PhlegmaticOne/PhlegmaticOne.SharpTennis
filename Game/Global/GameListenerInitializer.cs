using PhlegmaticOne.SharpTennis.Game.Common.Input;
using PhlegmaticOne.SharpTennis.Game.Game.Commands.Global;
using PhlegmaticOne.SharpTennis.Game.Game.Global.Base;
using SharpDX.DirectInput;

namespace PhlegmaticOne.SharpTennis.Game.Game.Global
{
    public class GameListenerInitializer : ListenerInitializer
    {
        private readonly GameEscapeButtonHandler _gameEscapeButtonHandler;

        public GameListenerInitializer(GlobalInputListener globalInputListener,
            GameEscapeButtonHandler gameEscapeButtonHandler) : base(globalInputListener)
        {
            _gameEscapeButtonHandler = gameEscapeButtonHandler;
        }

        public override void Initialize()
        {
            GlobalInputListener.AddListener(Key.Escape, _gameEscapeButtonHandler);
        }
    }
}
