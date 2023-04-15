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

        public event Action<Vector3> Moved;
        public event Action<Vector3> Scaled;
        public event Action<Vector3> Rotated; 

        public static Transform Identity(GameObject keeper)
        {
            var transform = new Transform(Vector3.Zero, Vector3.Zero, Vector3.One);
            transform.Transform = transform;
            transform.GameObject = keeper;
            return transform;
        }

        public static Transform EmptyIdentity => new Transform(Vector3.Zero, Vector3.Zero, Vector3.One);


        public Transform(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            _position = position;
            _rotation = rotation;
            _scale = scale;
        }

        public void Move(Vector3 position, bool invokeEvent = true)
        {
            _position += position;

            if (invokeEvent)
            {
                OnMoved(position);
            }
        }

        public void SetPosition(Vector3 position)
        {
            var diff = Diff(ref position, ref _position);
            _position = position;
            OnMoved(diff);
        }

        public void Rotate(Vector3 rotation, bool invokeEvent = true)
        {
            _rotation += rotation;
            ClampRotation(ref _rotation);

            if (invokeEvent)
            {
                OnRotated(rotation);
            }
        }

        public void SetRotation(Vector3 rotation)
        {
            var diff = Diff(ref _rotation, ref rotation);
            _rotation = rotation;
            ClampRotation(ref _rotation);
            OnRotated(diff);
        }

        public void ScaleBy(Vector3 scale)
        {
            _scale += scale;
            OnScaled(scale);
        }

        public void SetScale(Vector3 scale)
        {
            var diff = Diff(ref _scale, ref scale);
            _scale = scale;
            OnScaled(diff);
        }

        public Vector3 NormalizedRotation(Vector3 rotation) => rotation * Pi / 180;

        public Matrix GetWorldMatrix() => 
            Matrix.Scaling(_scale) * GetRotationMatrix() * Matrix.Translation(_position);

        public Matrix GetRotationMatrix()
        {
            var normalized = NormalizedRotation(_rotation);
            return Matrix.RotationYawPitchRoll(normalized.X, normalized.Y, normalized.Z);
        }

        private void OnMoved(Vector3 newPosition) => Moved?.Invoke(newPosition);
        private void OnScaled(Vector3 newScale) => Scaled?.Invoke(newScale);
        private void OnRotated(Vector3 newRotation) => Rotated?.Invoke(newRotation);

        private void ClampRotation(ref Vector3 rotation)
        {
            ClampAngle(ref rotation.X);
            ClampAngle(ref rotation.Y);
            ClampAngle(ref rotation.Z);
        }

        private static Vector3 Diff(ref Vector3 v1, ref Vector3 v2) => v1 - v2;

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
