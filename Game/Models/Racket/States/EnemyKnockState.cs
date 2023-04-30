using PhlegmaticOne.SharpTennis.Game.Common.StateMachine;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Racket.States
{
    public class EnemyKnockState : IStateBehavior
    {
        private readonly Vector3 _followToPosition;
        private readonly float _lerp;
        private readonly EnemyRacket _racket;
        private readonly BallModel _ball;
        private bool _performed;

        public EnemyKnockState(Vector3 followToPosition, float lerp, EnemyRacket racket, BallModel ball)
        {
            _followToPosition = followToPosition;
            _lerp = lerp;
            _racket = racket;
            _ball = ball;
        }

        public void Update()
        {
            var positionLerp = Vector3.Lerp(_racket.Transform.Position, _followToPosition, _lerp);
            _racket.Transform.SetPosition(positionLerp);

            if (!_performed)
            {
                _performed = true;
                _racket.ShakeInTime(false, new Vector3(10, 0, 0), _followToPosition - new Vector3(30, 0, 0), 1f, 0.3f,
                    () =>
                    {
                        _racket.Knock(_ball);
                    });
            }
        }
    }
}
