using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Sound.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Racket.Kicks;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Racket
{
    public class PlayerRacket : RacketBase
    {
        private KnockComponent _knockComponent;
        private KickComponent _kickComponent;
        private BoxCollider3D _boxCollider;
        public PlayerRacket(MeshComponent coloredComponent, MeshComponent handComponent,
            ISoundManager<GameSounds> soundManager) : 
            base(coloredComponent, handComponent, soundManager) { }

        protected override RacketType BallBounceType => RacketType.Player;

        public override void Start()
        {
            _knockComponent = GameObject.GetComponent<KnockComponent>();
            _kickComponent = GameObject.GetComponent<KickComponent>();
            _boxCollider = GameObject.GetComponent<BoxCollider3D>();
            base.Start();
        }

        protected override void OnCollisionWithBall(BallModel ballModel)
        {
            _boxCollider.DisableOnTime(0.3f);

            if (ballModel.BallGameState == BallGameState.None)
            {
                KnockBall(ballModel);
                return;
            }

            KickBall(ballModel);
        }

        private void KnockBall(BallModel ballModel)
        {
            ballModel.BouncedFromRacket = BallBounceType;
            ballModel.BallGameState = BallGameState.Knocked;
            var direction = RigidBody3D.Speed.Normalized();
            var force = RigidBody3D.Speed.Length();
            _knockComponent.KnockBall(ballModel, direction, force);
        }

        private void KickBall(BallModel ball)
        {
            ball.BallGameState = BallGameState.InPlay;
            var direction = RigidBody3D.Speed.Normalized();
            var force = RigidBody3D.Speed.Length();

            if (direction == Vector3.Zero)
            {
                var newSpeed = Collider.Reflect(ball.GetSpeed(), Normal, ball.Bounciness);
                ball.BounceDirect(this, newSpeed);
                return;
            }

            _kickComponent.KickBall(ball, direction, force);
        }
    }
}
