using System;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Common.Base
{
    public class Transform : Component
    {
        private const float Pi = (float)Math.PI;

        private Vector3 _position;
        private Vector3 _rotation;
        private Vector3 _scale;

        public Vector3 Position => _position;
        public Vector3 Rotation => _rotation;
        public Vector3 Scale => _scale;

        public static Transform Identity(GameObject keeper)
        {
            var transform = new Transform(Vector3.Zero, Vector3.Zero, Vector3.One);
            transform.Transform = transform;
            transform.GameObject = keeper;
            return transform;
        }

        public Transform(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            _position = position;
            _rotation = rotation;
            _scale = scale;
        }

        public void Move(Vector3 position) => _position += position;

        public void SetPosition(Vector3 position) => _position = position;

        public void Rotate(Vector3 rotation)
        {
            _rotation += rotation;
            ClampRotation();
        }

        public void SetRotation(Vector3 rotation)
        {
            _rotation = rotation;
            ClampRotation();
        }

        public void ScaleBy(Vector3 scale) => _scale += scale;

        public void SetScale(Vector3 scale) => _scale = scale;

        public Vector3 NormalizedRotation => _rotation * Pi / 180;

        public Matrix GetWorldMatrix() => 
            Matrix.Scaling(_scale) * GetRotationMatrix() * Matrix.Translation(_position);

        public Matrix GetRotationMatrix()
        {
            var normalized = NormalizedRotation;
            return Matrix.RotationYawPitchRoll(normalized.X, normalized.Y, normalized.Z);
        }

        private void ClampRotation()
        {
            ClampAngle(ref _rotation.X);
            ClampAngle(ref _rotation.Y);
            ClampAngle(ref _rotation.Z);
        }

        private static void ClampAngle(ref float angle)
        {
            if (angle > 180)
            {
                angle -= 360;
            }
            else if (angle < -180)
            {
                angle += 360;
            }
        }
    }
}
