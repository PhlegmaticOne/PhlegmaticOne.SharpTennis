using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Render;
using PhlegmaticOne.SharpTennis.Game.Engine3D;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Game.Scenes.Base;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Scenes
{
    public class GameSceneBuilder : ISceneBuilder
    {
        private readonly TextureMaterialsProvider _textureMaterialsProvider;
        private readonly MeshLoader _meshLoader;

        public GameSceneBuilder(TextureMaterialsProvider textureMaterialsProvider,
            MeshLoader meshLoader)
        {
            _textureMaterialsProvider = textureMaterialsProvider;
            _meshLoader = meshLoader;
        }

        public Scene BuildScene()
        {
            var scene = new Scene();
            LoadMaterials();
            scene.Camera = BuildCamera();
            BuildModels(scene);
            return scene;
        }

        public void BuildModels(Scene scene)
        {
            var sky = _meshLoader.LoadFbx("assets\\models\\sky.fbx",
                _textureMaterialsProvider.DefaultTexture)[0];
            sky.Transform.SetRotation(new Vector3(0, -90, 0));
            scene.AddGameObject(sky.GameObject);
            //var table = _meshLoader.LoadFbx("assets\\models\\table.fbx", _textureMaterialsProvider.DefaultTexture);

            //foreach (var meshComponent in table)
            //{
            //    scene.AddGameObject(meshComponent.GameObject);
            //}
        }

        private Camera BuildCamera()
        {
            var cameraObject = new GameObject();
            var camera = new Camera(farClipPlane:2000);
            cameraObject.AddComponent(camera);
            camera.Transform.SetPosition(new Vector3(0, 5, -5));
            return camera;
        }

        private void LoadMaterials()
        {
            var texture = _meshLoader.LoadTextureFromFile("assets\\textures\\white.png", false);
            //var emmisiveMaterial = new Material(texture, 
            //    new Vector3(1.0f), new Vector3(0.0f), new Vector3(0.0f), new Vector3(0.0f), 1.0f);
            var defaultMaterial = new Material(texture,
                new Vector3(0.0f), new Vector3(1.0f), new Vector3(1.0f), new Vector3(1.0f), 1.0f);

            _textureMaterialsProvider.SetDefaultMaterial(defaultMaterial);
            _textureMaterialsProvider.SetDefaultTexture(texture);
            //_textureMaterialsProvider.AddMaterial("Emmisive", emmisiveMaterial);
        }
    }
}
