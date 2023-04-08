using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Base;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Racket
{
    public class RacketFactory : IFactory<Racket>
    {
        private readonly MeshLoader _meshLoader;
        private readonly TextureMaterialsProvider _textureMaterialsProvider;

        public RacketFactory(MeshLoader meshLoader, TextureMaterialsProvider textureMaterialsProvider)
        {
            _meshLoader = meshLoader;
            _textureMaterialsProvider = textureMaterialsProvider;
        }

        public Racket Create(Transform transform)
        {
            var racket = _meshLoader.LoadFbx("assets\\models\\racket.fbx", _textureMaterialsProvider.DefaultTexture);

            foreach (var component in racket)
            {
                component.GameObject.Name = "Racket";
                component.Transform.Move(transform.Position);
                component.Transform.Rotate(transform.Rotation);
                component.Transform.SetScale(transform.Scale);
            }

            racket[0].Transform.Move(new Vector3(0.01f, 0, 0));

            var go = new GameObject("Racket");
            var model = new Racket(racket[0], racket[1]);
            go.AddComponent(model);
            return model;
        }
    }
}
