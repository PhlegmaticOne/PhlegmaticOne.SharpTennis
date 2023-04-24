using System;
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
            if (ballModel.IsInGame == false)
            {
                KnockBall(ballModel);
                return;
            }

            var racketSpeed = RigidBody3D.Speed.Normalized();
            var ballSpeed = ballModel.GetSpeed();

            if (racketSpeed == Vector3.Zero)
            {
                var newSpeed = Collider.Reflect(ballSpeed, Normal, ballModel.Bounciness);
                ballModel.BounceDirect(this, newSpeed);
                return;
            }

            var force = 100;
            var reflected = RigidBody3D.Speed;
            reflected.Y = GetY();
            ballModel.BounceDirect(this, reflected);
        }

        private float GetY() => Random.Next(30, 50);

        private void KnockBall(BallModel ballModel)
        {
            ballModel.BouncedFromRacket = BallBounceType;
            ballModel.IsInGame = true;

            var racketSpeed = RigidBody3D.Speed / 1.5f;

            if (racketSpeed.Y > 0)
            {
                racketSpeed.Y *= -1f;
            }

            ballModel.BounceDirect(this, racketSpeed);
        }
    }
}
