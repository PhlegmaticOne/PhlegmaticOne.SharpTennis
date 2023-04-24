using System;
using System.Collections.Generic;
using PhlegmaticOne.SharpTennis.Game.Common.Base;

namespace PhlegmaticOne.SharpTennis.Game.Common.StateMachine
{
    public class StateComponent : BehaviorObject
    {
        private readonly Dictionary<State, Func<IStateBehavior>> _states;

        private IStateBehavior _currentState;
        public StateComponent()
        {
            _states = new Dictionary<State, Func<IStateBehavior>>();
            AddState(State.None, () => new EmptyState());
        }

        public void AddState(State state, Func<IStateBehavior> stateFactory) => _states.Add(state, stateFactory);
        public void Enter(State state) => _currentState = _states[state]();
        public void Exit() => Enter(State.None);
        protected override void Update() => _currentState?.Update();
    }
}
