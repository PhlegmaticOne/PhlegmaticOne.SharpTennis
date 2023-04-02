namespace PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh
{
    public class MeshRendererData
    {
        public MeshRendererData(ShaderInfo vertexShaderInfo, ShaderInfo pixelShaderInfo)
        {
            VertexShaderInfo = vertexShaderInfo;
            PixelShaderInfo = pixelShaderInfo;
        }

        public ShaderInfo VertexShaderInfo { get; }
        public ShaderInfo PixelShaderInfo { get; }
    }

    public class ShaderInfo
    {
        public string ShaderPath { get; }
        public string EntryPoint { get; }
        public string Profile { get; }

        public ShaderInfo(string shaderPath, string entryPoint, string profile)
        {
            ShaderPath = shaderPath;
            EntryPoint = entryPoint;
            Profile = profile;
        }
    }
}
