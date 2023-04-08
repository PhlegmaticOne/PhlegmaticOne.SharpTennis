using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Infrastructure;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Engine3D.Rigid
{
    public class RigidBody3D : BehaviorObject
    {
        public Vector3 Speed { get; private set; }
        public RigidBodyType RigidBodyType { get; }

        public RigidBody3D(Vector3 speed, RigidBodyType rigidBodyType = RigidBodyType.Static)
        {
            if (rigidBodyType == RigidBodyType.Static)
            {
                speed = Vector3.Zero;
            }

            Speed = speed;
            RigidBodyType = rigidBodyType;
        }

        public bool IsDynamic => RigidBodyType == RigidBodyType.Dynamic;

        protected override void Update()
        {
            if (IsDynamic == false)
            {
                return;
            }
            Transform.Move(Speed * Time.DeltaT);
        }

        public void InverseSpeed()
        {
            if (IsDynamic == false)
            {
                return;
            }
            Speed *= -1;
        }
    }

    public enum RigidBodyType
    {
        Static,
        Dynamic
    }
}
