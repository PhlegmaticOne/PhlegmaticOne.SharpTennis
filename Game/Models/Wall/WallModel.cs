using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Wall
{
    public class WallModel : BehaviorObject
    {
        public MeshComponent Mesh { get; }
        public Vector3 Normal { get; set; } = Vector3.Left;

        public WallModel(MeshComponent mesh) => Mesh = mesh;

        public override void OnCollisionEnter(Collider other)
        {
            if (other.GameObject.TryGetComponent<BallModel>(out var ball))
            {
                ball.Bounce(this, Normal);
            }
        }
    }
}
