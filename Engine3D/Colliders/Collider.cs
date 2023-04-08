using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Rigid;

namespace PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders
{
    public abstract class Collider : BehaviorObject
    {
        protected RigidBody3D RigidBody;
        public abstract bool Intersects(Collider other);
        public override void Start()
        {
            RigidBody = GameObject.GetComponent<RigidBody3D>();
        }

        public void OnCollisionEnter(Collider other)
        {
            RigidBody.InverseSpeed();
            other.RigidBody.InverseSpeed();
        }
    }
}
