using System;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Floor;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Controllers
{
    public class BallFloorCollisionController : BehaviorObject
    {
        private readonly FloorModel _floorModel;

        public BallFloorCollisionController(FloorModel floorModel)
        {
            _floorModel = floorModel;
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
            ReturnToStatPositionRandom();
        }
    }
}
