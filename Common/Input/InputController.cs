using System;
using System.Linq;
using SharpDX;
using SharpDX.DirectInput;
using SharpDX.Windows;

namespace PhlegmaticOne.SharpTennis.Game.Common.Input
{
    public class InputController : IDisposable
    {
        private readonly bool[] _mouseButtons = new bool[8];
        private DirectInput _directInput;
        private Keyboard _keyboard;
        private KeyboardState _keyboardState;
        private bool _keyboardUpdated;
        private bool _keyboardAcquired;

        private Mouse _mouse;
        private MouseState _mouseState;
        private bool _mouseAcquired;
        private bool _mouseUpdated;
        public bool KeyboardUpdated => _keyboardUpdated;
        public bool MouseUpdated => _mouseUpdated;

        private int _mouseRelativePositionX;
        private int _mouseRelativePositionY;
        private int _mouseRelativePositionZ;

        public int MouseRelativePositionX => _mouseRelativePositionX;
        public int MouseRelativePositionY => _mouseRelativePositionY;
        public int MouseRelativePositionZ => _mouseRelativePositionZ;

        private readonly Key[] KeyFuncCodes;
        private readonly bool[] _keyFuncPreviousPressed;
        private readonly bool[] _keyFuncCurrentPressed;
        private readonly bool[] _keyFunc;

        public event Action<Key> Pressed;

        public InputController(RenderForm renderForm)
        {
            _directInput = new DirectInput();

            _keyboard = new Keyboard(_directInput);
            _keyboard.SetCooperativeLevel(renderForm.Handle, CooperativeLevel.Foreground | CooperativeLevel.NonExclusive);
            AcquireKeyboard();
            _keyboardState = new KeyboardState();

            _mouse = new Mouse(_directInput);
            _mouse.SetCooperativeLevel(renderForm.Handle, CooperativeLevel.Foreground | CooperativeLevel.Exclusive);
            AcquireMouse();
            _mouseState = new MouseState();

            KeyFuncCodes = Enum.GetValues(typeof(Key)).Cast<Key>().ToArray();
            _mousePressed = false;
            _keyFuncPreviousPressed = new bool[KeyFuncCodes.Length];
            _keyFuncCurrentPressed = new bool[KeyFuncCodes.Length];
            _keyFunc = new bool[KeyFuncCodes.Length];
        }


        private bool _mousePressed;
        public bool MouseLeft
        {
            get
            {
                if (!_mousePressed && _mouseButtons[0])
                {
                    _mousePressed = true;
                    return true;
                }

                if (_mousePressed && !_mouseButtons[0])
                {
                    _mousePressed = false;
                    return false;
                }

                return false;
            }
        }

        public bool IsPressed(Key key)
        {
            return _keyboardState.IsPressed(key);
        }

        public bool MouseRight
        {
            get
            {
                if (!_mousePressed && _mouseButtons[1])
                {
                    _mousePressed = true;
                    return true;
                }

                if (_mousePressed && !_mouseButtons[1])
                {
                    _mousePressed = false;
                    return false;
                }

                return false;
            }
        }


        private void AcquireKeyboard()
        {
            try
            {
                _keyboard.Acquire();
                _keyboardAcquired = true;
            }
            catch (SharpDXException e)
            {
                if (e.ResultCode.Failure)
                {
                    _keyboardAcquired = false;
                }
            }
        }

        private void AcquireMouse()
        {
            try
            {
                _mouse.Acquire();
                _mouseAcquired = true;
            }
            catch (SharpDXException e)
            {
                if (e.ResultCode.Failure)
                {
                    _mouseAcquired = false;
                }
            }
        }

        private bool TriggerByKeyUp(Key key, ref bool previous, ref bool current)
        {
            previous = current;
            current = _keyboardState.IsPressed(key);
            var result = previous && !current;

            if (result)
            {
                Pressed?.Invoke(key);
            }

            return result;
        }

        private bool TriggerByKeyDown(Key key, ref bool previous, ref bool current)
        {
            previous = current;
            current = _keyboardState.IsPressed(key);
            return !previous && current;
        }

        private void ProcessKeyboardState()
        {
            for (var i = 0; i < KeyFuncCodes.Length; ++i)
            {
                _keyFunc[i] = TriggerByKeyUp(KeyFuncCodes[i], ref _keyFuncPreviousPressed[i],
                    ref _keyFuncCurrentPressed[i]);
            }
        }

        public void UpdateKeyboardState()
        {
            if (!_keyboardAcquired)
            {
                AcquireKeyboard();
            }
            var resultCode = ResultCode.Ok;
            try
            {
                _keyboard.GetCurrentState(ref _keyboardState);
                ProcessKeyboardState();
                _keyboardUpdated = true;
            }
            catch (SharpDXException e)
            {
                resultCode = e.Descriptor;
                _keyboardUpdated = false;
            }

            if (resultCode == ResultCode.InputLost || resultCode == ResultCode.NotAcquired)
            {
                _keyboardAcquired = false;
            }
        }

        private void ProcessMouseState()
        {
            for (int i = 0; i <= 7; ++i)
            {
                _mouseButtons[i] = _mouseState.Buttons[i];
            }

            _mouseRelativePositionX = _mouseState.X;
            _mouseRelativePositionY = _mouseState.Y;
            _mouseRelativePositionZ = _mouseState.Z;
        }

        public void UpdateMouseState()
        {
            if (!_mouseAcquired) AcquireMouse();
            var resultCode = ResultCode.Ok;
            try
            {
                _mouse.GetCurrentState(ref _mouseState);
                ProcessMouseState();
                _mouseUpdated = true;
            }
            catch (SharpDXException e)
            {
                resultCode = e.Descriptor;
                _mouseUpdated = false;
            }

            if (resultCode == ResultCode.InputLost || resultCode == ResultCode.NotAcquired)
            {
                _mouseAcquired = false;
            }
        }

        public void Dispose()
        {
            _mouse.Unacquire();
            _keyboard.Unacquire();
            Utilities.Dispose(ref _mouse);
            Utilities.Dispose(ref _keyboard);
            Utilities.Dispose(ref _directInput);
        }
    }
}
