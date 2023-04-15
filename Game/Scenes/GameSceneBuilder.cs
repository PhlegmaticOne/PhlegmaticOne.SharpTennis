using System.Collections.Generic;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Floor;
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
        private readonly BallFactory _ballFactory;
        private readonly FloorFactory _floorFactory;

        public GameSceneBuilder(TextureMaterialsProvider textureMaterialsProvider,
            MeshLoader meshLoader)
        {
            _textureMaterialsProvider = textureMaterialsProvider;
            _meshLoader = meshLoader;
            _tennisTableFactory = new TennisTableFactory(meshLoader, textureMaterialsProvider);
            _racketFactory = new RacketFactory(_meshLoader, _textureMaterialsProvider);
            _ballFactory = new BallFactory(_meshLoader, textureMaterialsProvider);
            _floorFactory = new FloorFactory(_meshLoader, textureMaterialsProvider);
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

            racket.Boxes = DrawCollider(scene, racket.GameObject.GetComponent<BoxCollider3D>());

            var ball = _ballFactory.Create(new Transform(
                new Vector3(-50, 20, 20), Vector3.Zero, Vector3.One));
            scene.AddGameObject(ball.Mesh.GameObject);
            scene.AddGameObject(ball.GameObject);

            var floor = _floorFactory.Create(new Transform(
                new Vector3(50, -30, 0),
                new Vector3(0, -90, 0),
                new Vector3(6, 3, 1f)));

            scene.AddGameObject(floor.Mesh.GameObject);
            scene.AddGameObject(floor.GameObject);
        }

        private List<MeshComponent> DrawCollider(Scene scene, BoxCollider3D boxCollider)
        {
            var list = new List<MeshComponent>();
            var corners = boxCollider.BoundingBox.GetCorners();
            foreach (var corner in corners)
            {
                var ball = _meshLoader.LoadFbx("assets\\models\\ball.fbx", _textureMaterialsProvider.DefaultTexture)[0];
                ball.Transform.SetPosition(corner);
                scene.AddGameObject(ball.GameObject);
                list.Add(ball);
            }
            return list;
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
    }
}
