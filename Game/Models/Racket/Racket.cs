using System;
using System.Collections.Generic;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Rigid;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using SharpDX;
using SharpDX.X3DAudio;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Racket
{
    public class Racket : BehaviorObject
    {
        private readonly MeshComponent _coloredComponent;
        private readonly MeshComponent _handComponent;
        private RigidBody3D _rigidBody3D;

        public Racket(MeshComponent coloredComponent, MeshComponent handComponent, bool isPlayer)
        {
            IsPlayer = isPlayer;
            _coloredComponent = coloredComponent;
            _handComponent = handComponent;
            Meshes = new List<MeshComponent> { _coloredComponent, _handComponent };
        }

        public bool IsPlayer { get; set; }
        public List<MeshComponent> Meshes { get; }
        public List<MeshComponent> Boxes { get; set; }
        public BoxCollider3D BoxCollider { get; private set; }
        public Vector3 Normal { get; set; }

        public override void Start()
        {
            Transform.SetPosition(_handComponent.Transform.Position);
            BoxCollider = GameObject.GetComponent<BoxCollider3D>();
            _rigidBody3D = GameObject.GetComponent<RigidBody3D>();
            Transform.Moved += TransformOnMoved;
            Transform.Rotated += TransformOnRotated;
        }

        public void Color(Color color)
        {
            var vector = new Vector3(color.R, color.G, color.B) / 255;
            var properties = _coloredComponent.MeshObjectData.Material.MaterialProperties;
            properties.SetColor(vector);
            _coloredComponent.MeshObjectData.Material.MaterialProperties = properties;
        }

        public void UpdateSpeed(Vector3 speed)
        {
            _rigidBody3D.Speed = speed;
        }

        public override void OnCollisionEnter(Collider other)
        {
            if (other.GameObject.TryGetComponent<BallModel>(out var ball))
            {
                SetBallBounce(ball);

                if (IsPlayer == false)
                {
                    var speed = ball.GetSpeed();
                    if (speed.X == 0)
                    {
                        return;
                    }
                    var newSpeed = new Vector3(
                        -speed.X, speed.Y + 30, speed.Z);
                    ball.SetSpeed(newSpeed);
                    return;
                }

                var s = _rigidBody3D.Speed.Normalized();

                if (s.X == 0)
                {
                    s.X = 1;
                }
                var force = 100;
                var reflected = force * s;
                reflected.Y = 50;
                ball.BounceDirect(this, reflected);
            }
        }

        private void SetBallBounce(BallModel ball)
        {
            ball.BallBounceType = IsPlayer ? BallBounceType.Player : BallBounceType.Enemy;
        }

        private void TransformOnRotated(Vector3 obj)
        {
            _coloredComponent.Transform.Rotate(obj);
            _handComponent.Transform.Rotate(obj);
            BoxCollider.Rotate(obj);
        }

        private void TransformOnMoved(Vector3 obj)
        {
            _coloredComponent.Transform.Move(obj);
            _handComponent.Transform.Move(obj);
        }
    }
}
