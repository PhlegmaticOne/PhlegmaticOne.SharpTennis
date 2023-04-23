using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Infrastructure
{
    public class TextureProviderFactory
    {
        private readonly MeshLoader _meshLoader;

        public TextureProviderFactory(MeshLoader meshLoader) => _meshLoader = meshLoader;

        public TextureMaterialsProvider CreateTextureProvider()
        {
            var result = new TextureMaterialsProvider();

            var texture = _meshLoader.LoadTextureFromFile("assets\\textures\\white.png", false);
            var defaultMaterial = new Material(texture,
                new Vector3(0.0f), new Vector3(1.0f), new Vector3(1.0f), new Vector3(1.0f), 1.0f);

            result.SetDefaultMaterial(defaultMaterial);
            result.SetDefaultTexture(texture);

            return result;
        }
    }
}
