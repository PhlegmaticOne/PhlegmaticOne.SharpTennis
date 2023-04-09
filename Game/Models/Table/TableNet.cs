using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Table
{
    public class TableNet : BehaviorObject
    {
        public Vector3 Normal { get; set; }

        public Vector3 GetNormal(BallModel model)
        {
            var speed = model.GetSpeed();

            if (speed.X > 0)
            {
                return Normal;
            }

            if (speed.X < 0)
            {
                return Normal * -1f;
            }

            return Vector3.Zero;
        }

        public override void OnCollisionEnter(Collider other)
        {
            if (other.GameObject.TryGetComponent<BallModel>(out var ball))
            {
                var ballSpeed = ball.GetSpeed();
                ball.SetSpeed(Vector3.Reflect(ballSpeed, GetNormal(ball)));
            }
        }
    }
}
