using System;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Floor;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Controllers
{
    public class BallFloorCollisionController : BehaviorObject
    {
        private readonly ScoreSystem _scoreSystem;

        public BallFloorCollisionController(ScoreSystem scoreSystem, BallBounceProvider ballBounceProvider)
        {
            _scoreSystem = scoreSystem;
            ballBounceProvider.BallBounced += BallBounceProviderOnBallBounced;
        }

        private void BallBounceProviderOnBallBounced(Component bouncedFrom, BallModel ball)
        {
            if (bouncedFrom.GameObject.HasComponent<FloorModel>())
            {
                AddScore(ball);
                ReturnToStatPositionRandom(ball);
            }
        }

        public void Setup(ScoreText playerText, ScoreText enemyText)
        {
            _scoreSystem.EnemyText = enemyText;
            _scoreSystem.PlayerText = playerText;
        }

        public void ReturnToStatPositionRandom(BallModel ball)
        {
            var z = new Random().Next(-20, 20);
            ball.RigidBody.EnableGravity();
            ball.SetSpeed(Vector3.Zero);
            ball.Transform.SetPosition(new Vector3(-50, 20, z));
        }

        private void AddScore(BallModel ball)
        {
            if (ball.BouncedFrom == BallBouncedFromType.Player)
            {
                _scoreSystem.AddScoreToPlayer(1);
            }
            else
            {
                _scoreSystem.AddScoreToEnemy(1);
            }
        }
    }
}
