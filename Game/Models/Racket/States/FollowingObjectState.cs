using PhlegmaticOne.SharpTennis.Game.Common.StateMachine;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Racket.States
{
    public class FollowingObjectState : IStateBehavior
    {
        private readonly Vector3 _followToPosition;
        private readonly float _lerp;
        private readonly EnemyRacket _racket;

        public FollowingObjectState(Vector3 followToPosition, float lerp, EnemyRacket racket)
        {
            _followToPosition = followToPosition;
            _lerp = lerp;
            _racket = racket;
        }

        public void Update()
        {
            if (_followToPosition == Vector3.Zero)
            {
                return;
            }

            var positionLerp = Vector3.Lerp(_racket.Transform.Position, _followToPosition, _lerp);
            _racket.Transform.SetPosition(positionLerp);
        }
    }
}
