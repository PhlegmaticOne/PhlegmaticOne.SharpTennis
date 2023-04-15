using System;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Floor
{
    public class FloorModel : BehaviorObject
    {
        public MeshComponent Mesh { get; }
        public BoxCollider3D Collider { get; private set; }

        public event Action<BallModel> BallHit; 

        public FloorModel(MeshComponent mesh)
        {
            Mesh = mesh;
        }

        public override void Start()
        {
            Collider = GameObject.GetComponent<BoxCollider3D>();
        }

        public override void OnCollisionEnter(Collider other)
        {
            if (other.GameObject.TryGetComponent<BallModel>(out var ball))
            {
                BallHit?.Invoke(ball);
            }
        }
    }
}
