using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Infrastructure;
using PhlegmaticOne.SharpTennis.Game.Common.Sound.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Table
{
    public class TableTopPart : BehaviorObject
    {
        private readonly ISoundManager<GameSounds> _soundManager;
        private readonly Vector3 _minPosition;
        private readonly Vector3 _maxPosition;

        private float _timeFromLastBounce;

        public Vector3 Normal { get; set; }
        public Vector2 Size => (Vector2)(_maxPosition - _minPosition);

        public TableTopPart(BoxCollider3D collider, ISoundManager<GameSounds> soundManager)
        {
            _soundManager = soundManager;
            _minPosition = collider.BoundingBox.Minimum;
            _maxPosition = collider.BoundingBox.Maximum;
        }

        public override void OnCollisionEnter(Collider other)
        {
            if (other.GameObject.TryGetComponent<BallModel>(out var ball))
            {
                TryPlayBounceSound();
                SetBounceTypeToBall(ball);
                ball.Bounce(this, Normal);
            }
        }

        private void TryPlayBounceSound()
        {
            var passedTime = Time.PassedTime;
            if (passedTime - _timeFromLastBounce >= 0.2f)
            {
                _soundManager.Play(GameSounds.TableBounce);
            }
            _timeFromLastBounce = passedTime;
        }

        private void SetBounceTypeToBall(BallModel ball)
        {
            var position = ball.Transform.Position;

            if (position.X > _minPosition.X && position.X < 0)
            {
                SetBounceType(ball, RacketType.Player);
                return;
            }

            if (position.X < _maxPosition.X && position.X > 0)
            {
                SetBounceType(ball, RacketType.Enemy);
            }
        }

        private static void SetBounceType(BallModel ball, RacketType ballBouncedFromType)
        {
            if (ball.BouncedFromTablePart == ballBouncedFromType)
            {
                ball.BouncedFromTableTimes++;
            }
            else
            {
                ball.BouncedFromTableTimes = 1;
            }
            ball.BouncedFromTablePart = ballBouncedFromType;
        }
    }
}
