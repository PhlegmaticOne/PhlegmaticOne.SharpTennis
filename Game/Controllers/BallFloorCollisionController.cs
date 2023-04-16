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
        private readonly FloorModel _floorModel;
        private readonly ScoreSystem _scoreSystem;

        public BallFloorCollisionController(FloorModel floorModel, ScoreSystem scoreSystem)
        {
            _floorModel = floorModel;
            _scoreSystem = scoreSystem;
            _floorModel.BallHit += FloorModelOnBallHit;
        }

        public void ReturnToStatPositionRandom()
        {
            var z = new Random().Next(-20, 20);
            var ball = Scene.Current.GetComponent<BallModel>();
            ball.RigidBody.EnableGravity();
            ball.SetSpeed(Vector3.Zero);
            ball.Transform.SetPosition(new Vector3(-50, 20, z));
        }

        private void FloorModelOnBallHit(BallModel obj)
        {
            AddScore(obj);
            ReturnToStatPositionRandom();
        }

        private void AddScore(BallModel ball)
        {
            if (ball.BallBounceType == BallBounceType.Player)
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
