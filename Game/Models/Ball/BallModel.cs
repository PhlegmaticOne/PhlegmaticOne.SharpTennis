using System.Threading.Tasks;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Rigid;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Ball
{
    public class BallModel : BehaviorObject
    {
        private RigidBody3D _rigidBody3D;
        private SphereCollider _sphereCollider;

        public BallModel(MeshComponent mesh)
        {
            Mesh = mesh;
        }

        public float Bounciness => _rigidBody3D.Bounciness;

        public MeshComponent Mesh { get; }

        public override void Start()
        {
            _rigidBody3D = GameObject.GetComponent<RigidBody3D>();
            _sphereCollider = GameObject.GetComponent<SphereCollider>();
            Transform.Moved += TransformOnMoved;
        }

        public Vector3 GetSpeed() => _rigidBody3D.Speed;

        public void SetSpeed(Vector3 speed) => _rigidBody3D.Speed = speed;

        private void TransformOnMoved(Vector3 obj)
        {
            Mesh.Transform.Move(obj);
        }

        public override void OnCollisionEnter(Collider collider)
        {
            if (collider.GameObject.HasComponent<Racket.Racket>() == false)
            {
                return;
            }

            _sphereCollider.ChangeEnabled(false);
            Task.Run(async () =>
            {
                await Task.Delay(100);
                _sphereCollider.ChangeEnabled(true);
            });
        }
    }
}
