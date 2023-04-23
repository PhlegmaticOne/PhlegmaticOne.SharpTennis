using PhlegmaticOne.SharpTennis.Game.Common.StateMachine;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Racket.States
{
    public class EnemyRacketStates : IStateCollection
    {
        public EnemyRacketStates()
        {
            StayingState = new State("Staying");
            FollowingBallState = new State("FollowingBallState");
            MovingToStartState = new State("MovingToStartState");
        }

        public State StayingState { get; }
        public State FollowingBallState { get; }
        public State MovingToStartState { get; }
    }
}
