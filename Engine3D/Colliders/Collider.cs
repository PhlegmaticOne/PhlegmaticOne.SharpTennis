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
            if (Enabled == false)
            {
                return;
            }

            foreach (var behaviorObject in GameObject.GetComponents<BehaviorObject>())
            {
                behaviorObject.OnCollisionEnter();
            }

            RigidBody?.InverseSpeed();
        }
    }
}
