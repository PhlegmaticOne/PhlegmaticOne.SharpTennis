using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Input;
using PhlegmaticOne.SharpTennis.Game.Engine3D;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Racket;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Controllers
{
    public class RacketMoveController : BehaviorObject
    {
        private readonly Racket _racket;
        private readonly Camera _camera;
        private readonly InputController _inputController;

        public RacketMoveController(Racket racket, Camera camera, InputController inputController)
        {
            _racket = racket;
            _camera = camera;
            _inputController = inputController;
        }

        private float _minX = -30;
        private float _maxX = -84;
        private float _minZ = -40;
        private float _maxZ = 40;

        protected override void Update()
        {
            if (_inputController.MouseUpdated)
            {
                var deltaX = -(float)_inputController.MouseRelativePositionX / 20;
                var deltaZ = -(float)_inputController.MouseRelativePositionY / 20;
                _racket.Transform.Move(new Vector3(deltaZ, 0, deltaX));

                var pos = _racket.Transform.Position;
                var moveCamera = true;

                if (pos.X > _minX || pos.X < _maxX)
                {
                    _racket.Transform.Move(new Vector3(-deltaZ, 0, 0));
                    moveCamera = false;
                }

                if (pos.Z < _minZ || pos.Z > _maxZ)
                {
                    _racket.Transform.Move(new Vector3(0, 0, -deltaX));
                    moveCamera = false;
                }

                Rotate();

                if (moveCamera)
                {
                    MoveCamera(deltaX, deltaZ);
                }
            }
        }

        private void Rotate()
        {
            var z = _racket.Transform.Position.Z;
            var r = _racket.Transform.Rotation;
            var angle = 180 * (_minZ - z) / (_maxZ - _minZ);
            _racket.Transform.SetRotation(new Vector3(r.X, r.Y, -angle - 90));
        }


        private void MoveCamera(float deltaX, float deltaZ)
        {
            _camera.Transform.Move(new Vector3(deltaZ, 0, deltaX) / 30);
        }
    }
}
