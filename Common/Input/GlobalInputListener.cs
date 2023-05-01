using System.Collections.Generic;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Commands;
using SharpDX.DirectInput;

namespace PhlegmaticOne.SharpTennis.Game.Common.Input
{
    public class GlobalInputListener : BehaviorObject
    {
        private readonly InputController _inputController;
        private readonly Dictionary<Key, ICommand> _commands = new Dictionary<Key, ICommand>();

        public GlobalInputListener(InputController inputController)
        {
            _inputController = inputController;
            _inputController.Pressed += InputControllerOnPressed;
        }

        private void InputControllerOnPressed(Key obj)
        {
            if (_commands.TryGetValue(obj, out var command) && command.CanExecute(null))
            {
                command.Execute(null);
            }
        }

        public override void Start() => DontDestroyOnLoad(GameObject);
        public void AddListener(Key key, ICommand command)
        {
            if (_commands.ContainsKey(key))
            {
                _commands[key] = command;
            }
            else
            {
                _commands.Add(key, command);
            }
        }

        public void RemoveAll() => _commands.Clear();
    }
}
