using System.Linq;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Sound.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Rigid;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Floor
{
    public class FloorFactory : IFactory<FloorModel>
    {
        private readonly MeshLoader _meshLoader;
        private readonly TextureMaterialsProvider _textureMaterialsProvider;
        private readonly ISoundManager<GameSounds> _soundManager;

        public FloorFactory(MeshLoader meshLoader, TextureMaterialsProvider textureMaterialsProvider,
            ISoundManager<GameSounds> soundManager)
        {
            _meshLoader = meshLoader;
            _textureMaterialsProvider = textureMaterialsProvider;
            _soundManager = soundManager;
        }

        public FloorModel Create(Transform transform)
        {
            var floor = _meshLoader.LoadFbx("assets\\models\\floor.fbx", _textureMaterialsProvider.DefaultTexture)[0];
            floor.Transform.InitializeFromTransform(transform);

            var go = new GameObject();
            var floorModel = new FloorModel(floor, _soundManager);
            go.AddComponent(floorModel);
            go.AddComponent(new RigidBody3D(Vector3.Zero));
            go.AddComponent(CreateCollider(floor.MeshObjectData, transform.Position, transform.Scale));
            return floorModel;
        }

        private BoxCollider3D CreateCollider(MeshObjectData mesh, Vector3 position, Vector3 scale)
        {
            var halfWidth = mesh.Vertices.Max(x => x.position.X) * scale.X;
            var halfHeight = mesh.Vertices.Max(x => x.position.Y) * scale.Y;

            var width = halfWidth * 2;
            var height = halfHeight * 2;
            var depth = 15;

            var a = position + new Vector3(halfWidth, 0, halfHeight);
            var b = a - new Vector3(width, depth, height);

            return new BoxCollider3D(b, a)
            {
                IsStatic = true
            };
        }
    }
}
