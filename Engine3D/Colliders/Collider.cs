using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Rigid;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders
{
    public abstract class Collider : BehaviorObject
    {
        private const float MinSpeed = 4f;
        protected RigidBody3D RigidBody;
        public bool IsColliding { get; private set; }

        public abstract bool Intersects(Collider other);
        public override void Start()
        {
            RigidBody = GameObject.GetComponent<RigidBody3D>();
        }

        public static Vector3 Reflect(Vector3 speed, Vector3 normal, float bounciness)
        {
            var reflected = Vector3.Reflect(speed, normal);
            return reflected * bounciness;
        }

        public void Collision(Collider other)
        {
            if (Enabled == false)
            {
                return;
            }

            IsColliding = true;
            ResolveOnCollisionSpeed();
            RaiseOnCollisionEnterInCurrentObject(other);
        }

        public void CollisionStay(Collider other)
        {
            if (Enabled == false)
            {
                return;
            }

            RaiseOnCollisionStayInCurrentObject(other);
        }

        public void CollisionExit(Collider other)
        {
            if (Enabled == false)
            {
                return;
            }

            IsColliding = false;
            RaiseOnCollisionExitInCurrentObject(other);
        }


        private void RaiseOnCollisionEnterInCurrentObject(Collider other)
        {
            foreach (var behaviorObject in GameObject.GetComponents<BehaviorObject>())
            {
                behaviorObject.OnCollisionEnter(other);
            }
        }

        private void RaiseOnCollisionStayInCurrentObject(Collider other)
        {
            foreach (var behaviorObject in GameObject.GetComponents<BehaviorObject>())
            {
                behaviorObject.OnCollisionStay(other);
            }
        }

        private void RaiseOnCollisionExitInCurrentObject(Collider other)
        {
            foreach (var behaviorObject in GameObject.GetComponents<BehaviorObject>())
            {
                behaviorObject.OnCollisionExit(other);
            }
        }

        private void ResolveOnCollisionSpeed()
        {
            if (RigidBody.RigidBodyType == RigidBodyType.Static)
            {
                return;
            }

            var length = RigidBody.Speed.Length();

            if (RigidBody.RigidBodyType == RigidBodyType.Dynamic && length < MinSpeed)
            {
                ChangeEnabled(false);
                RigidBody.Stop();
            }
        }
    }
}
