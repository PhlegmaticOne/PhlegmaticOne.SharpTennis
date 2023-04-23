using Assimp;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Base;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Sky
{
    public class SkyFactory : IFactory<SkyModel>
    {
        private readonly TextureMaterialsProvider _textureMaterialsProvider;
        private readonly MeshLoader _meshLoader;

        public SkyFactory(TextureMaterialsProvider textureMaterialsProvider, MeshLoader meshLoader)
        {
            _textureMaterialsProvider = textureMaterialsProvider;
            _meshLoader = meshLoader;
        }

        public SkyModel Create(Transform transform)
        {
            var mesh = _meshLoader.LoadFbx("assets\\models\\sky.fbx",
                _textureMaterialsProvider.DefaultTexture)[0];
            mesh.Transform.InitializeFromTransform(transform);

            var go = new GameObject("Sky");
            var sky = new SkyModel();
            sky.AddMeshes(mesh);
            go.AddComponent(sky);
            return sky;
        }
    }
}
