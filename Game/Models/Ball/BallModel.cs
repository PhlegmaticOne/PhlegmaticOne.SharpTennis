using System.Threading.Tasks;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Rigid;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Ball
{
    public enum BallBounceType
    {
        Player,
        Enemy
    }

    public class BallModel : BehaviorObject
    {
        private SphereCollider _sphereCollider;

        public BallModel(MeshComponent mesh)
        {
            Mesh = mesh;
        }

        public float Bounciness => RigidBody.Bounciness;

        public MeshComponent Mesh { get; }
        public RigidBody3D RigidBody { get; private set; }
        public BallBounceType BallBounceType { get; set; }

        public override void Start()
        {
            RigidBody = GameObject.GetComponent<RigidBody3D>();
            _sphereCollider = GameObject.GetComponent<SphereCollider>();
            Transform.Moved += TransformOnMoved;
        }

        public Vector3 GetSpeed() => RigidBody.Speed;

        public void SetSpeed(Vector3 speed) => RigidBody.Speed = speed;

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
                await Task.Delay(200);
                _sphereCollider.ChangeEnabled(true);
            });
        }
    }
}
