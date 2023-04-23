using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Table
{
    public class TableTopPart : BehaviorObject
    {
        private Vector3 _minPosition;
        private Vector3 _maxPosition;

        public Vector3 Normal { get; set; }
        public Vector2 Size => (Vector2)(_maxPosition - _minPosition);

        public override void Start()
        {
            var collider = GameObject.GetComponent<BoxCollider3D>();
            _minPosition = collider.BoundingBox.Minimum;
            _maxPosition = collider.BoundingBox.Maximum;
        }

        public override void OnCollisionEnter(Collider other)
        {
            if (other.GameObject.TryGetComponent<BallModel>(out var ball))
            {
                SetBounceTypeToBall(ball);
                ball.Bounce(this, Normal);
            }
        }

        private void SetBounceTypeToBall(BallModel ball)
        {
            var position = ball.Transform.Position;

            if (position.X > _minPosition.X && position.X < 0)
            {
                ball.BouncedFrom = BallBouncedFromType.Player;
            }

            if (position.X < _maxPosition.X && position.X > 0)
            {
                ball.BouncedFrom = BallBouncedFromType.Enemy;
            }

            ball.BouncedFromTableTimes++;
        }
    }
}
