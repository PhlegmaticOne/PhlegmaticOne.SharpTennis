using System;
using System.Collections.Generic;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Rigid;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Racket
{
    public class Racket : BehaviorObject
    {
        private readonly MeshComponent _coloredComponent;
        private readonly MeshComponent _handComponent;
        private RigidBody3D _rigidBody3D;

        public Racket(MeshComponent coloredComponent, MeshComponent handComponent)
        {
            _coloredComponent = coloredComponent;
            _handComponent = handComponent;
            Meshes = new List<MeshComponent> { _coloredComponent, _handComponent };
        }

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
                var s = _rigidBody3D.Speed.Normalized();
                var force = 100;
                var speed = ball.GetSpeed();
                var reflected = force * s;
                reflected.Y = 50;
                ball.SetSpeed(reflected);
            }
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
