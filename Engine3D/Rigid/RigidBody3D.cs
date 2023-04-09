using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Infrastructure;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Engine3D.Rigid
{
    public class RigidBody3D : BehaviorObject
    {
        public static float GlobalAcceleration = -1f;
        public Vector3 Speed { get; set; }
        public Vector3 Acceleration { get; set; }
        public RigidBodyType RigidBodyType { get; private set; }
        public float Bounciness { get; set; } = 1f;

        public RigidBody3D(Vector3 speed, RigidBodyType rigidBodyType = RigidBodyType.Static)
        {
            if (rigidBodyType == RigidBodyType.Static)
            {
                Speed = Vector3.Zero;
                Acceleration = Vector3.Zero;
            }
            else
            {
                Speed = speed;
                Acceleration = new Vector3(0, GlobalAcceleration, 0);
            }

            RigidBodyType = rigidBodyType;
        }

        public void Stop()
        {
            Acceleration = Vector3.Zero;
            Speed = Vector3.Zero;
        }

        public void EnableGravity()
        {
            Acceleration = new Vector3(0, GlobalAcceleration, 0);
        }

        public bool IsDynamic => RigidBodyType == RigidBodyType.Dynamic;

        protected override void Update()
        {
            if (IsDynamic == false)
            {
                return;
            }

            Speed += Acceleration;
            Transform.Move(Speed * Time.DeltaT);
        }
    }

    public enum RigidBodyType
    {
        Static,
        Kinematic,
        Dynamic
    }
}
