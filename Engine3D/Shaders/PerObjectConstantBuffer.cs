using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct3D11;

namespace PhlegmaticOne.SharpTennis.Game.Engine3D.Shaders
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PerObjectData
    {
        public Matrix worldViewProjectionMatrix;
        public Matrix worldMatrix;
        public Matrix inverseTransposeWorldMatrix;
        public int timeScaling;
        public Vector3 _padding;
    }

    public class PerObjectConstantBuffer : ConstantBuffer<PerObjectData>
    {
        public PerObjectConstantBuffer(Device device, DeviceContext deviceContext, CommonShaderStage commonShaderStage, int subresource, int slot) : 
            base(device, deviceContext, commonShaderStage, subresource, slot) { }

        public void Update(Matrix world, Matrix view, Matrix projection, int timeScaling)
        {

            Data.worldViewProjectionMatrix = Matrix.Multiply(Matrix.Multiply(world, view), projection);
            Data.worldViewProjectionMatrix.Transpose();
            Data.timeScaling = timeScaling;
            Data.worldMatrix = world;
            Data.worldMatrix.Transpose();
            Data.inverseTransposeWorldMatrix = Matrix.Invert(world);

            Update();
        }
    }
}
