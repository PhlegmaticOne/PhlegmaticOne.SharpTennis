using System.Collections.Generic;
using System.Linq;
using Assimp;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Rigid;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Floor;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Racket;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Table;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Wall;
using PhlegmaticOne.SharpTennis.Game.Game.Scenes.Base;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct2D1.Effects;
using Camera = PhlegmaticOne.SharpTennis.Game.Engine3D.Camera;
using Material = PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh.Material;
using Scene = PhlegmaticOne.SharpTennis.Game.Common.Base.Scene;
using Transform = PhlegmaticOne.SharpTennis.Game.Common.Base.Transform;

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
            scene.AddGameObject(table.GameObject);

            AddRacket(scene, new Transform(
                new Vector3(-70, 10, 0),
                new Vector3(90, -180, -90),
                new Vector3(1, 1, 1)), Color.Red, Vector3.Right, true);

            AddRacket(scene, new Transform(
                new Vector3(70, 10, 0),
                new Vector3(90, -180, -90),
                new Vector3(1, 1, 1)), Color.Black, Vector3.Left, false);

            var ball = _ballFactory.Create(new Transform(
                new Vector3(-50, 20, 20), Vector3.Zero, Vector3.One));
            scene.AddGameObject(ball.Mesh.GameObject);
            scene.AddGameObject(ball.GameObject);

            var floor = _floorFactory.Create(new Transform(
                new Vector3(50, -30, 0),
                new Vector3(0, -90, 0),
                new Vector3(8, 5, 1f)));

            scene.AddGameObject(floor.Mesh.GameObject);
            scene.AddGameObject(floor.GameObject);

            //AddWall(scene, new Transform(
            //    new Vector3(50, 0, 0), new Vector3(90, 180, 180), Vector3.One));
        }


        private void AddWall(Scene scene, Transform transform)
        {
            var wall = _meshLoader.LoadFbx("assets\\models\\floor.fbx", _textureMaterialsProvider.DefaultTexture)[0];

            wall.Transform.SetRotation(transform.Rotation);
            wall.Transform.SetPosition(transform.Position);

            var go = new GameObject();
            go.Transform.SetPosition(transform.Position);
            go.Transform.SetRotation(transform.Rotation);
            go.AddComponent(new WallModel(wall));
            go.AddComponent(new RigidBody3D(Vector3.Zero));
            go.AddComponent(CreateCollider(wall.MeshObjectData, transform.Position));

            scene.AddGameObject(wall.GameObject);
            scene.AddGameObject(go);
            DrawCollider(scene, go.GetComponent<BoxCollider3D>());
        }

        private BoxCollider3D CreateCollider(MeshObjectData mesh, Vector3 position)
        {
            var halfWidth = mesh.Vertices.Max(x => x.position.X);
            var halfHeight = mesh.Vertices.Max(x => x.position.Y);

            var width = halfWidth * 2;
            var height = halfHeight * 2;
            var depth = 5;

            var a = position - new Vector3(0, halfHeight, halfWidth);
            var b = a + new Vector3(depth, height, width);

            return new BoxCollider3D(a, b)
            {
                IsStatic = true
            };
        }


        private void AddRacket(Scene scene, Transform transform, Color color, Vector3 normal, bool isPlayer)
        {
            var racket = _racketFactory.Create(transform, new RacketFactoryData
            {
                Color = color,
                IsPlayer = isPlayer,
                Normal = normal
            });

            scene.AddGameObject(racket.GameObject);
            foreach (var meshComponent in racket.Meshes)
            {
                scene.AddGameObject(meshComponent.GameObject);
            }
            //racket.Boxes = DrawCollider(scene, racket.GameObject.GetComponent<BoxCollider3D>());
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
    }
}
