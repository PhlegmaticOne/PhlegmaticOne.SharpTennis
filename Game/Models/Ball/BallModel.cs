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
        public MeshComponent Mesh { get; }

        public override void Start()
        {
            _rigidBody3D = GameObject.GetComponent<RigidBody3D>();
            _sphereCollider = GameObject.GetComponent<SphereCollider>();
            Transform.Moved += TransformOnMoved;
        }

        private void TransformOnMoved(Vector3 obj)
        {
            Mesh.Transform.Move(obj);
        }

        public override void OnCollisionEnter()
        {
            _sphereCollider.ChangeEnabled(false);
            Task.Run(async () =>
            {
                await Task.Delay(1000);
                _sphereCollider.ChangeEnabled(true);
            });
        }
    }
}
