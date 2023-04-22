using System;
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
        None,
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

        public event Action<Component, BallModel> Bounced; 

        public override void Start()
        {
            RigidBody = GameObject.GetComponent<RigidBody3D>();
            _sphereCollider = GameObject.GetComponent<SphereCollider>();
            Transform.Moved += TransformOnMoved;
        }


        public void Bounce(Component bouncedFrom, Vector3 normal, float bounciness = 0)
        {
            var ballSpeed = GetSpeed();
            var reflected = Collider.Reflect(ballSpeed, normal, bounciness == 0f ? Bounciness : bounciness);
            SetSpeed(reflected);
            Bounced?.Invoke(bouncedFrom, this);
        }

        public void BounceDirect(Component bouncedFrom, Vector3 newSpeed)
        {
            SetSpeed(newSpeed);
            Bounced?.Invoke(bouncedFrom, this);
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
                await Task.Delay(400);
                _sphereCollider.ChangeEnabled(true);
            });
        }
    }
}
