namespace PhlegmaticOne.SharpTennis.Game.Common.StateMachine
{
    public interface IStateHandler
    {
        State CurrentState { get; }
        void EnterState(State state);
        void Exit();
    }
}
