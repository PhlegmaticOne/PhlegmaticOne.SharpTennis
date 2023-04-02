using System;
using SharpDX;
using SharpDX.Direct3D11;

namespace PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh
{
    public class Texture : IDisposable
    {
        private Texture2D _textureObject;

        private ShaderResourceView _shaderResourceView;
        public ShaderResourceView ShaderResourceView => _shaderResourceView;
        public int Height { get; }
        public int Width { get; }
        public SamplerState SamplerState { get; }

        public Texture(Texture2D textureObject, ShaderResourceView shaderResourceView, 
            int width, int height, SamplerState samplerState)
        {
            _textureObject = textureObject;
            _shaderResourceView = shaderResourceView;
            Width = width;
            Height = height;
            SamplerState = samplerState;
        }

        public void Dispose()
        {
            Utilities.Dispose(ref _shaderResourceView);
            Utilities.Dispose(ref _textureObject);
        }
    }
}
