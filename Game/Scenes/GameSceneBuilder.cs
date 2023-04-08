using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Racket;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Table;
using PhlegmaticOne.SharpTennis.Game.Game.Scenes.Base;
using SharpDX;
using Camera = PhlegmaticOne.SharpTennis.Game.Engine3D.Camera;
using Material = PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh.Material;
using Scene = PhlegmaticOne.SharpTennis.Game.Common.Base.Scene;

namespace PhlegmaticOne.SharpTennis.Game.Game.Scenes
{
    public class GameSceneBuilder : ISceneBuilder
    {
        private readonly TextureMaterialsProvider _textureMaterialsProvider;
        private readonly MeshLoader _meshLoader;
        private readonly TennisTableFactory _tennisTableFactory;
        private readonly RacketFactory _racketFactory;

        public GameSceneBuilder(TextureMaterialsProvider textureMaterialsProvider,
            MeshLoader meshLoader)
        {
            _textureMaterialsProvider = textureMaterialsProvider;
            _meshLoader = meshLoader;
            _tennisTableFactory = new TennisTableFactory(meshLoader, textureMaterialsProvider);
            _racketFactory = new RacketFactory(_meshLoader, _textureMaterialsProvider);
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

            var table = _tennisTableFactory.Create(Transform.EmptyIdentity);

            foreach (var mesh in table.Meshes)
            {
                scene.AddGameObject(mesh.GameObject);
            }

            var racket = _racketFactory.Create(new Transform(
                new Vector3(-70, 10, 0),
                new Vector3(90, -180, -90),
                new Vector3(1, 1, 1)));
            racket.Color(Color.Red);
            scene.AddGameObject(racket.GameObject);
            foreach (var meshComponent in racket.Meshes)
            {
                scene.AddGameObject(meshComponent.GameObject);
            }


            //var ball = _meshLoader.LoadFbx("assets\\models\\ball.fbx", _textureMaterialsProvider.DefaultTexture)[0];
            //ball.GameObject.Name = "Ball";
            //var radius = ball.MeshObjectData.Vertices.Max(x => x.position.Z);
            //ball.GameObject.AddComponent(new SphereCollider(new Vector3(0, 50, 0), radius));
            //ball.GameObject.AddComponent(new RigidBody3D(new Vector3(0, -4, 0), RigidBodyType.Dynamic));
            //ball.Transform.SetPosition(new Vector3(0, 50, 0));
            //scene.AddGameObject(ball.GameObject);
        }

        private void DrawCollider(Scene scene, BoxCollider3D boxCollider)
        {
            foreach (var corner in boxCollider.BoundingBox.GetCorners())
            {
                var ball = _meshLoader.LoadFbx("assets\\models\\ball.fbx", _textureMaterialsProvider.DefaultTexture)[0];
                ball.Transform.SetPosition(corner);
                scene.AddGameObject(ball.GameObject);
            }
        }

        private Camera BuildCamera()
        {
            var cameraObject = new GameObject("MainCamera");
            var camera = new Camera(farClipPlane:2000, fovY:0.9f);
            cameraObject.AddComponent(camera);
            camera.Transform.SetPosition(new Vector3(-110, 37, 0));
            camera.Transform.SetRotation(new Vector3(90, 25, 0));
            return camera;
        }

        private void LoadMaterials()
        {
            var texture = _meshLoader.LoadTextureFromFile("assets\\textures\\white.png", false);
            var defaultMaterial = new Material(texture,
                new Vector3(0.0f), new Vector3(1.0f), new Vector3(1.0f), new Vector3(1.0f), 1.0f);

            _textureMaterialsProvider.SetDefaultMaterial(defaultMaterial);
            _textureMaterialsProvider.SetDefaultTexture(texture);
        }

        private void Swap(ref Vector4 a, float increaseZBy)
        {
            var z = a.Z;
            a.Z = a.Y;
            a.Z += increaseZBy;
            a.X += increaseZBy;
            a.Y = z;
        }

        private void Swap(ref Vector4 a)
        {
            var z = a.Z;
            a.Z = a.Y;
            a.Y = z;
        }
    }
}
