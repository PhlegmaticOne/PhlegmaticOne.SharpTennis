using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Infrastructure;
using PhlegmaticOne.SharpTennis.Game.Common.Input;
using PhlegmaticOne.SharpTennis.Game.Engine3D;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Racket;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Controllers
{
    public class RacketMoveController : BehaviorObject
    {
        #region Reg

        private readonly float _minX = -30;
        private readonly float _maxX = -94;
        private readonly float _minZ = -40;
        private readonly float _maxZ = 40;
        private readonly float _minY = 6;
        private readonly float _maxY = 12;

        #endregion

        private readonly InputController _inputController;
        private RacketBase _racket;
        private Camera _camera;
        private BallModel _ball;


        public RacketMoveController(InputController inputController)
        {
            _inputController = inputController;
        }

        public void Setup(RacketBase racket, Camera camera, BallModel ball)
        {
            _racket = racket;
            _camera = camera;
            _ball = ball;
            ChangeEnabled(true);
        }

        public float MousePositionDivider { get; set; } = 30f;


        protected override void Update()
        {
            if (_inputController.MouseUpdated == false || Enabled == false)
            {
                return;
            }

            var deltaMove = GetMouseDeltaMove();
            deltaMove.Y = MoveDownOrUp();
            _racket.Transform.Move(deltaMove);
            deltaMove.Y = 0;
            _racket.UpdateSpeed(deltaMove / Time.DeltaT);

            if (TryMoveBackRacket(deltaMove))
            {
                MoveCamera(deltaMove.Z, deltaMove.Y, deltaMove.X);
            }

            TryRotateBall(deltaMove);
        }

        private void TryRotateBall(Vector3 deltaMove)
        {
            if (_ball.BouncedFromRacket == RacketType.Player && _ball.BouncedFromTablePart != RacketType.Enemy)
            {
                var z = deltaMove.Z;
                var y = deltaMove.X / 1.5f;
                _ball.SetSpeed(_ball.GetSpeed() + new Vector3(0, y, z));
            }
        }

        private float MoveDownOrUp()
        {
            var delta = 0.1f;

            if (_inputController.MouseRight)
            {
                return -1 * delta;
            }


            if (_inputController.MouseLeft)
            {
                return delta;
            }

            return 0;
        }

        private Vector3 GetMouseDeltaMove()
        {
            var deltaX = -(float)_inputController.MouseRelativePositionX / MousePositionDivider;
            var deltaZ = -(float)_inputController.MouseRelativePositionY / MousePositionDivider;
            return new Vector3(deltaZ, 0, deltaX);
        }

        private bool TryMoveBackRacket(Vector3 mouseDeltaMove)
        {
            var pos = _racket.Transform.Position;
            var moveCamera = true;

            if (pos.Y > _maxY || pos.Y < _minY)
            {
                _racket.Transform.Move(new Vector3(0, -mouseDeltaMove.Y, 0));
                moveCamera = false;
            }

            if (pos.X > _minX || pos.X < _maxX)
            {
                _racket.Transform.Move(new Vector3(-mouseDeltaMove.X, 0, 0));
                moveCamera = false;
            }

            if (pos.Z < _minZ || pos.Z > _maxZ)
            {
                _racket.Transform.Move(new Vector3(0, 0, -mouseDeltaMove.Z));
                moveCamera = false;
            }

            return moveCamera;
        }

        private void MoveCamera(float deltaX, float deltaZ, float deltaY) => 
            _camera.Transform.Move(new Vector3(deltaZ, 0, deltaX) / 10);
    }
}
