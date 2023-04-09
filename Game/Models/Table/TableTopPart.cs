using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Table
{
    public class TableTopPart : BehaviorObject
    {
        public Vector3 Normal { get; set; }

        public override void OnCollisionEnter(Collider other)
        {
            if (other.GameObject.TryGetComponent<BallModel>(out var ball))
            {
                var ballSpeed = ball.GetSpeed();
                var reflected = Collider.Reflect(ballSpeed, Normal, ball.Bounciness);
                ball.SetSpeed(reflected);
            }
        }
    }
}
