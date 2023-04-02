using PhlegmaticOne.SharpTennis.Game.Common.Base;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Engine3D
{
    public class Camera : BehaviorObject
    {
        public float FovY { get; set;  }
        public float Aspect { get; set; }
        public float NearClipPlane { get; }
        public float FarClipPlane { get; }

        public Camera(float fovY = MathUtil.PiOverFour, float aspect = 1.0f,
            float nearClipPlane = 0.1f, float farClipPlane = 1000f)
        {
            FovY = fovY;
            Aspect = aspect;
            NearClipPlane = nearClipPlane;
            FarClipPlane = farClipPlane;
        }

        public Matrix GetProjectionMatrix() => 
            Matrix.PerspectiveFovLH(FovY, Aspect, NearClipPlane, FarClipPlane);

        public Matrix GetViewMatrix()
        {
            var position = Transform.Position;
            var rotation = Transform.GetRotationMatrix();
            var viewTo = (Vector3)Vector4.Transform(Vector4.UnitZ, rotation);
            var viewUp = (Vector3)Vector4.Transform(Vector4.UnitY, rotation);
            return Matrix.LookAtLH(position, position + viewTo, viewUp);
        }
    }
}
