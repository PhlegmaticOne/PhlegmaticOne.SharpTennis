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

        private void FloorModelOnBallHit(BallModel obj)
        {
            obj.SetSpeed(Vector3.Zero);
            obj.Transform.SetPosition(new Vector3(-50, 20, 20));
        }
    }
}
