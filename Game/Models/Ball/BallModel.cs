using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Rigid;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Base;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Ball
{
    public enum RacketType
    {
        None,
        Player,
        Enemy
    }

    public enum BallGameState
    {
        None,
        Knocked,
        InPlay
    }

    public class BallModel : MeshableObject
    {
        private Vector3 _lastSpeed;
        private Vector3 _startPosition;

        private readonly MeshComponent _mesh;
        private readonly BallBounceProvider _ballBounceProvider;

        public BallModel(MeshComponent mesh, BallBounceProvider ballBounceProvider)
        {
            _mesh = mesh;
            _ballBounceProvider = ballBounceProvider;
            BouncedFromTablePart = RacketType.Player;
            BallGameState = BallGameState.None;
            AddMeshes(mesh);
        }

        public float Bounciness => RigidBody.Bounciness;
        public RigidBody3D RigidBody { get; private set; }
        public RacketType BouncedFromTablePart { get; set; }
        public RacketType BouncedFromRacket { get; set; }
        public BallGameState BallGameState { get; set; }
        public int BouncedFromTableTimes { get; set; }


        public override void Start()
        {
            RigidBody = GameObject.GetComponent<RigidBody3D>();
            _startPosition = Transform.Position;
            Transform.Moved += TransformOnMoved;
        }

        public void Reset()
        {
            SetSpeed(Vector3.Zero);
            Transform.SetPosition(_startPosition);
        }

        public void ChangeActive(bool active)
        {
            if (active == false)
            {
                _lastSpeed = GetSpeed();
                SetSpeed(Vector3.Zero);
                RigidBody.DisableGravity();
            }
            else
            {
                SetSpeed(_lastSpeed);
                RigidBody.EnableGravity();
            }
        }

        public void Bounce(Component bouncedFrom, Vector3 normal, float bounciness = 0)
        {
            var ballSpeed = GetSpeed();
            var reflected = Collider.Reflect(ballSpeed, normal, bounciness == 0f ? Bounciness : bounciness);
            SetSpeed(reflected);
            _ballBounceProvider.OnBallBounced(bouncedFrom, this);
        }

        public void BounceDirect(Component bouncedFrom, Vector3 newSpeed)
        {
            SetSpeed(newSpeed);
            _ballBounceProvider.OnBallBounced(bouncedFrom, this);
        }

        public Vector3 GetSpeed() => RigidBody.Speed;

        public void SetSpeed(Vector3 speed) => RigidBody.Speed = speed;

        private void TransformOnMoved(Vector3 obj) => _mesh.Transform.Move(obj);
    }
}
