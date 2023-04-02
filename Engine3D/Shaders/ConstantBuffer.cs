using System;
using SharpDX;
using SharpDX.Direct3D11;
using Buffer11 = SharpDX.Direct3D11.Buffer;

namespace PhlegmaticOne.SharpTennis.Game.Engine3D.Shaders
{
    public class ConstantBuffer<T> : IDisposable where T : struct
    {
        private readonly DeviceContext _deviceContext;
        private readonly CommonShaderStage _commonShaderStage;
        private readonly int _subResource;
        private readonly int _slot;

        protected T Data;
        private Buffer11 _buffer;

        public ConstantBuffer(Device device, DeviceContext deviceContext, CommonShaderStage commonShaderStage, int subResource, int slot)
        {
            _deviceContext = deviceContext;
            _buffer = new Buffer11(device,
                Utilities.SizeOf<T>(),
                ResourceUsage.Dynamic,
                BindFlags.ConstantBuffer,
                CpuAccessFlags.Write,
                ResourceOptionFlags.None, 0);
            _commonShaderStage = commonShaderStage;
            _subResource = subResource;
            _slot = slot;
        }


        protected void Update()
        {
            _deviceContext.MapSubresource(_buffer, MapMode.WriteDiscard, MapFlags.None, out var dataStream);
            dataStream.Write(Data);
            _deviceContext.UnmapSubresource(_buffer, _subResource);
            _commonShaderStage.SetConstantBuffer(_slot, _buffer);
        }

        public void Dispose() => Utilities.Dispose(ref _buffer);
    }
}
