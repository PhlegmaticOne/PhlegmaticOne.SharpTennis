using PhlegmaticOne.SharpTennis.Game.Common.Input;
using PhlegmaticOne.SharpTennis.Game.Game.Commands.Global;
using PhlegmaticOne.SharpTennis.Game.Game.Global.Base;
using SharpDX.DirectInput;

namespace PhlegmaticOne.SharpTennis.Game.Game.Global
{
    public class MenuListenerInitializer : ListenerInitializer
    {
        private readonly MenuEscapeButtonHandler _menuEscapeButtonHandler;
        public MenuListenerInitializer(GlobalInputListener globalInputListener, MenuEscapeButtonHandler menuEscapeButtonHandler) : base(globalInputListener)
        {
            _menuEscapeButtonHandler = menuEscapeButtonHandler;
        }

        public override void Initialize()
        {
            GlobalInputListener.AddListener(Key.Escape, _menuEscapeButtonHandler);
        }
    }
}
