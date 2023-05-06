using System;
using System.Collections.Generic;
using PhlegmaticOne.SharpTennis.Game.Common.Sound.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Rigid;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Racket
{
    public abstract class RacketBase : MeshableObject
    {
        protected static readonly Random Random = new Random();
        private Vector3 _startPosition;
        private const float MinZ = -40;
        private const float MaxZ = 40;

        private readonly MeshComponent _coloredComponent;
        private readonly MeshComponent _handComponent;
        private readonly ISoundManager<GameSounds> _soundManager;
        protected RigidBody3D RigidBody3D;

        protected RacketBase(MeshComponent coloredComponent, MeshComponent handComponent,
            ISoundManager<GameSounds> soundManager)
        {
            _coloredComponent = coloredComponent;
            _handComponent = handComponent;
            _soundManager = soundManager;
            AddMeshes(_coloredComponent, _handComponent);
        }

        public BoxCollider3D BoxCollider { get; private set; }
        public Vector3 Normal { get; set; }
        public ColorType ColorType { get; set; }

        public override void Start()
        {
            Transform.SetPosition(_handComponent.Transform.Position);
            BoxCollider = GameObject.GetComponent<BoxCollider3D>();
            RigidBody3D = GameObject.GetComponent<RigidBody3D>();
            Transform.Moved += TransformOnMoved;
            Transform.Rotated += TransformOnRotated;
            _startPosition = Transform.Position;
        }

        public virtual void Reset() => Transform.SetPosition(_startPosition);

        public void Color(ColorType colorType)
        {
            ColorType = colorType;
            var color = colorType == ColorType.Red ? SharpDX.Color.Red : SharpDX.Color.Black;
            Color(color);
            //var vector = new Vector3(color.R, color.G, color.B) / 255;
            //var properties = _coloredComponent.MeshObjectData.Material.MaterialProperties;
            //properties.SetColor(vector);
            //_coloredComponent.MeshObjectData.Material.MaterialProperties = properties;
        }

        public void Color(Color color)
        {
            var properties = _coloredComponent.MeshObjectData.Material.MaterialProperties;
            properties.SetColor(new Vector3(color.R, color.G, color.B) / 255);
            _coloredComponent.MeshObjectData.Material.MaterialProperties = properties;
        }

        public void UpdateSpeed(Vector3 speed) => RigidBody3D.Speed = speed;

        public override void OnCollisionEnter(Collider other)
        {
            if (other.GameObject.TryGetComponent<BallModel>(out var ball))
            {
                _soundManager.Play(GameSounds.RacketBounce);
                SetBallBounce(ball);
                OnCollisionWithBall(ball);
            }
        }

        protected abstract RacketType BallBounceType { get; }
        protected abstract void OnCollisionWithBall(BallModel ballModel);
        public abstract void SetupDifficulty(DifficultyType difficulty);
        public virtual void OnLost(BallModel ball) { }
        public virtual void OnBallBounced(BallModel ball) { }

        private void SetBallBounce(BallModel ball)
        {
            ball.BouncedFromRacket = BallBounceType;
            ball.BouncedFromTableTimes = 0;
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
            Rotate();
        }

        private void Rotate()
        {
            var z = Transform.Position.Z;
            var r = Transform.Rotation;
            var angle = 180 * (MinZ - z) / (MaxZ - MinZ);
            Transform.SetRotation(new Vector3(r.X, r.Y, -angle - 90));
        }
    }
}
