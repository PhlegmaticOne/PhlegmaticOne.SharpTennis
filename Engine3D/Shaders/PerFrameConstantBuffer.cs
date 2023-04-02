using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct3D11;

namespace PhlegmaticOne.SharpTennis.Game.Engine3D.Shaders
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PerFrameData
    {
        public float time;
        public Vector3 _padding;
    }

    public class PerFrameConstantBuffer : ConstantBuffer<PerFrameData>
    {
        public PerFrameConstantBuffer(Device device, DeviceContext deviceContext, CommonShaderStage commonShaderStage, int subresource, int slot) 
            : base(device, deviceContext, commonShaderStage, subresource, slot) { }

        public void Update(float time)
        {
            Data.time = time;
            Update();
        }
    }
}
