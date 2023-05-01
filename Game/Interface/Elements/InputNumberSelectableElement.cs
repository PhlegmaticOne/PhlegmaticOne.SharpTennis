using PhlegmaticOne.SharpTennis.Game.Common.Input;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using SharpDX.DirectInput;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements
{
    public class InputNumberSelectableElement : SelectableComponent<string>
    {
        private readonly InputController _inputController;

        public InputNumberSelectableElement(InputController inputController)
        {
            _inputController = inputController;
            _inputController.Pressed += InputControllerOnPressed;
        }

        private void InputControllerOnPressed(Key key)
        {
            if (IsSelected == false)
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

            var num = KeysToNumberStringParser.Parse(key);
            UpdateValue(Value + num);
        }

        public int NumberValue
        {
            get 
            {
                if (int.TryParse(Value, out var result))
                {
                    return result;
                }

                return -1;
            }
        }
    }

    public static class KeysToNumberStringParser
    {
        public static string Parse(Key key)
        {
            switch (key)
            {
                case Key.D0: return "0";
                case Key.D1: return "1"; 
                case Key.D2: return "2";
                case Key.D3: return "3";
                case Key.D4: return "4";
                case Key.D5: return "5";
                case Key.D6: return "6";
                case Key.D7: return "7";
                case Key.D8: return "8";
                case Key.D9: return "9";
                default: return string.Empty;
            }
        }
    }
}
