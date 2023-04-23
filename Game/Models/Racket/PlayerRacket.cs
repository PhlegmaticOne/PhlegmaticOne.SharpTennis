using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Racket
{
    public class PlayerRacket : RacketBase
    {
        public PlayerRacket(MeshComponent coloredComponent, MeshComponent handComponent) : 
            base(coloredComponent, handComponent) { }

        protected override BallBouncedFromType BallBounceType => BallBouncedFromType.Player;

        protected override void OnCollisionWithBall(BallModel ballModel)
        {
            ballModel.IsInGame = true;
            var racketSpeed = RigidBody3D.Speed.Normalized();
            var ballSpeed = ballModel.GetSpeed();

            if (racketSpeed == Vector3.Zero)
            {
                var newSpeed = Collider.Reflect(ballSpeed, Normal, ballModel.Bounciness);
                ballModel.SetSpeed(newSpeed);
                return;
            }

            var force = 100;
            var reflected = force * racketSpeed;
            reflected.Y = 50;
            ballModel.BounceDirect(this, reflected);
        }
    }
}
