using System.Threading.Tasks;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Rigid;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Base;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Ball
{
    public enum BallBouncedFromType
    {
        None,
        Player,
        Enemy
    }

    public class BallModel : MeshableObject
    {
        private readonly MeshComponent _mesh;
        private readonly BallBounceProvider _ballBounceProvider;
        private SphereCollider _sphereCollider;
        private BallBouncedFromType _currentBallBouncedFromType;

        public BallModel(MeshComponent mesh, BallBounceProvider ballBounceProvider)
        {
            _mesh = mesh;
            _ballBounceProvider = ballBounceProvider;
            _currentBallBouncedFromType = BallBouncedFromType.None;
            AddMeshes(mesh);
        }

        public float Bounciness => RigidBody.Bounciness;

        public RigidBody3D RigidBody { get; private set; }

        public BallBouncedFromType BouncedFrom
        { 
            get => _currentBallBouncedFromType;
            set
            {
                IsPreviousBounceTypeDifferent = value != _currentBallBouncedFromType;
                _currentBallBouncedFromType = value;
            }
        }

        public bool IsPreviousBounceTypeDifferent { get; private set; }
        public bool IsInGame { get; set; }
        public int BouncedFromTableTimes { get; set; }

        public override void Start()
        {
            RigidBody = GameObject.GetComponent<RigidBody3D>();
            _sphereCollider = GameObject.GetComponent<SphereCollider>();
            Transform.Moved += TransformOnMoved;
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

        public override void OnCollisionEnter(Collider collider)
        {
            if (collider.GameObject.HasComponent<Racket.RacketBase>() == false)
            {
                return;
            }

            _sphereCollider.ChangeEnabled(false);
            Task.Run(async () =>
            {
                await Task.Delay(400);
                _sphereCollider.ChangeEnabled(true);
            });
        }
    }
}
