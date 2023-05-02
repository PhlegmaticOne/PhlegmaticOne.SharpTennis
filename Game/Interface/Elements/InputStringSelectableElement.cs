using PhlegmaticOne.SharpTennis.Game.Common.Input;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using SharpDX.DirectInput;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements
{
    public class InputStringSelectableElement : SelectableComponent<string>
    {
        private readonly InputController _inputController;

        public InputStringSelectableElement(InputController inputController)
        {
            _inputController = inputController;
            _inputController.Pressed += InputControllerOnPressed;
        }

        public int MaxStringLength { get; set; } = 10;

        private void InputControllerOnPressed(Key key)
        {
            if (IsSelected == false || Value.Length >= MaxStringLength)
            {
                return;
            }

            if (key == Key.Back)
            {
                if (Value.Length == 0)
                {
                    return;
                }
                var newValue = Value.Substring(0, Value.Length - 1);
                UpdateValue(newValue);
                return;
            }

            if (key.ToString().Length > 1)
            {
                return;
            }

            UpdateValue(Value + key.ToString().ToLower());
        }
    }
}
