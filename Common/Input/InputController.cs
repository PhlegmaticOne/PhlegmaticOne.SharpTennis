using System;
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

        private static readonly Key[] KeyFuncCodes =
        {
            Key.F1, Key.F2, Key.F3, Key.F4, Key.F5, Key.F6, Key.F7, Key.F8, Key.F9, Key.F10
        };
        private readonly bool[] _keyFuncPreviousPressed = new bool[10];
        private readonly bool[] _keyFuncCurrentPressed = new bool[10];
        private readonly bool[] _keyFunc = new bool[10];


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

        public InputController(RenderForm renderForm)
        {
            _directInput = new DirectInput();

            _keyboard = new Keyboard(_directInput);
            _keyboard.SetCooperativeLevel(renderForm.Handle, CooperativeLevel.Foreground | CooperativeLevel.NonExclusive);
            AcquireKeyboard();
            _keyboardState = new KeyboardState();

            _mouse = new Mouse(_directInput);
            _mouse.SetCooperativeLevel(renderForm.Handle, CooperativeLevel.Foreground | CooperativeLevel.NonExclusive);
            AcquireMouse();
            _mouseState = new MouseState();

            _mousePressed = false;
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
            return previous && !current;
        }

        private bool TriggerByKeyDown(Key key, ref bool previous, ref bool current)
        {
            previous = current;
            current = _keyboardState.IsPressed(key);
            return !previous && current;
        }

        private void ProcessKeyboardState()
        {
            for (var i = 0; i <= 9; ++i)
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
