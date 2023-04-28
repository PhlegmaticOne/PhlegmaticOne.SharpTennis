using System.Collections.Generic;
using System.Linq;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Base.Scenes;
using PhlegmaticOne.SharpTennis.Game.Common.Input;
using PhlegmaticOne.SharpTennis.Game.Common.Tween;
using PhlegmaticOne.SharpTennis.Game.Engine2D;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Game.Controllers;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Floor;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Racket;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Sky;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Table;
using SharpDX;
using Camera = PhlegmaticOne.SharpTennis.Game.Engine3D.Camera;
using Scene = PhlegmaticOne.SharpTennis.Game.Common.Base.Scenes.Scene;
using Transform = PhlegmaticOne.SharpTennis.Game.Common.Base.Transform;

namespace PhlegmaticOne.SharpTennis.Game.Game.Scenes
{
    public class GameSceneBuilder : ISceneBuilder
    {
        private readonly IFactory<TennisTable> _tennisTableFactory;
        private readonly IFactory<RacketBase, RacketFactoryData> _racketFactory;
        private readonly IFactory<BallModel> _ballFactory;
        private readonly IFactory<FloorModel> _floorFactory;
        private readonly IFactory<SkyModel> _skyFactory;
        private readonly InputController _inputController;
        private readonly SceneProvider _sceneProvider;
        private readonly BallBounceProvider _ballBounceProvider;
        private readonly MeshLoader _meshLoader;
        private readonly TextureMaterialsProvider _textureMaterialsProvider;

        public GameSceneBuilder(IFactory<TennisTable> tennisTableFactory,
            IFactory<RacketBase, RacketFactoryData> racketFactory,
            IFactory<BallModel> ballFactory,
            IFactory<FloorModel> floorFactory,
            IFactory<SkyModel> skyFactory,
            InputController inputController,
            SceneProvider sceneProvider,
            BallBounceProvider ballBounceProvider,
            MeshLoader meshLoader,
            TextureMaterialsProvider textureMaterialsProvider)
        {
            _tennisTableFactory = tennisTableFactory;
            _racketFactory = racketFactory;
            _ballFactory = ballFactory;
            _floorFactory = floorFactory;
            _skyFactory = skyFactory;
            _inputController = inputController;
            _sceneProvider = sceneProvider;
            _ballBounceProvider = ballBounceProvider;
            _meshLoader = meshLoader;
            _textureMaterialsProvider = textureMaterialsProvider;
        }

        public Scene BuildScene()
        {
            var scene = new Scene
            {
                Camera = BuildCamera()
            };
            BuildModels(scene);
            BuildControllers(scene);
            return scene;
        }

        public void BuildModels(Scene scene)
        {
            var sky = _skyFactory.Create(new Transform(rotation: new Vector3(0, -90, 0)));
            AddMeshableObject(scene, sky);

            var table = _tennisTableFactory.Create(Transform.EmptyIdentity);
            AddMeshableObject(scene, table);

            AddRacket(scene, new Transform(
                new Vector3(-70, 9, 0),
                new Vector3(90, -180, -90),
                new Vector3(1, 1, 1)), Color.Red, Vector3.Right, true, table.TableTopPart);

            AddRacket(scene, new Transform(
                new Vector3(70, 9, 0),
                new Vector3(90, -180, -90),
                new Vector3(1, 1, 1)), Color.Black, Vector3.Left, false, table.TableTopPart);

            var ball = _ballFactory.Create(new Transform(position: new Vector3(-50, 20, 20)));
            AddMeshableObject(scene, ball);

            var floor = _floorFactory.Create(new Transform(
                new Vector3(50, -30, 0),
                new Vector3(0, -90, 0),
                new Vector3(8, 5, 1f)));
            AddMeshableObject(scene, floor);
        }

        private void BuildControllers(Scene scene)
        {
            AddViewComponents(scene);
            AddGameStateChecker(scene);
            AddDoTween(scene);
            AddFloorCollisionController(scene);
            AddPlayerRacketMoveController(scene);
            AddPhysicSystems(scene);
        }

        private void AddViewComponents(Scene scene)
        {
            scene.AddGameObject(CreateGameObjectWithComponent("ScoreSystem", new ScoreSystem()));
            scene.AddGameObject(CreateGameObjectWithComponent("GameStateView", new GameStateViewController()));
        }

        private void AddGameStateChecker(Scene scene)
        {
            var scoreSystem = scene.GetComponent<ScoreSystem>();
            var gameStateView = scene.GetComponent<GameStateViewController>();
            scene.AddGameObject(CreateGameObjectWithComponent("GameStateChecker", new BallBouncesController(
                _ballBounceProvider, scoreSystem, gameStateView)));
        }

        private void AddDoTween(Scene scene)
        {
            scene.AddGameObject(DoTweenManager.Instance.GameObject);
        }

        private void AddPhysicSystems(Scene scene)
        {
            scene.AddGameObject(CreateGameObjectWithComponent("CollisionSystem",
                new CollidingSystem(_sceneProvider)));
        }

        private void AddPlayerRacketMoveController(Scene scene)
        {
            var playerRacket = scene.GetComponent<PlayerRacket>();
            scene.AddGameObject(CreateGameObjectWithComponent("PlayerRacketController",
                new RacketMoveController(playerRacket, scene.Camera, _inputController)));
        }

        private void AddFloorCollisionController(Scene scene)
        {
            var scoreSystem = scene.GetComponent<ScoreSystem>();
            scene.AddGameObject(CreateGameObjectWithComponent("FloorCollisionController", 
                new BallFloorCollisionController(scoreSystem, _ballBounceProvider)));
        }

        private GameObject CreateGameObjectWithComponent<T>(string name, T component) where T : Component
        {
            var go = new GameObject(name);
            go.AddComponent(component);
            return go;
        }


        private void AddRacket(Scene scene, Transform transform, Color color, Vector3 normal, bool isPlayer,
            TableTopPart tableTopPart)
        {
            var racket = _racketFactory.Create(transform, new RacketFactoryData
            {
                Color = color,
                IsPlayer = isPlayer,
                Normal = normal,
                TableHeight = tableTopPart.Size.X / 2,
                TableNormal = tableTopPart.Normal
            });

            AddMeshableObject(scene, racket);
            //racket.Boxes = DrawCollider(scene, racket.GameObject.GetComponent<BoxCollider3D>());
        }

        private static RectangleF RetrieveTableRect(BoxCollider3D tableCollider, bool isPlayer)
        {
            var box = tableCollider.BoundingBox;
            return new RectangleF();
        }

        private static void AddMeshableObject(Scene scene, MeshableObject meshableObject)
        {
            foreach (var meshableObjectMesh in meshableObject.Meshes)
            {
                scene.AddGameObject(meshableObjectMesh.GameObject);
            }

            scene.AddGameObject(meshableObject.GameObject);
        }


        private static Camera BuildCamera()
        {
            var cameraObject = new GameObject("MainCamera");
            var camera = new Camera(farClipPlane:2000, fovY:0.9f);
            cameraObject.AddComponent(camera);
            camera.Transform.SetPosition(new Vector3(-120, 37, 0));
            camera.Transform.SetRotation(new Vector3(90, 25, 0));
            return camera;
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


//private void AddWall(Scene scene, Transform transform)
//{
//    var wall = _meshLoader.LoadFbx("assets\\models\\floor.fbx", _textureMaterialsProvider.DefaultTexture)[0];

//    wall.Transform.SetRotation(transform.Rotation);
//    wall.Transform.SetPosition(transform.Position);

//    var go = new GameObject();
//    go.Transform.SetPosition(transform.Position);
//    go.Transform.SetRotation(transform.Rotation);
//    go.AddComponent(new WallModel(wall));
//    go.AddComponent(new RigidBody3D(Vector3.Zero));
//    go.AddComponent(CreateCollider(wall.MeshObjectData, transform.Position));

//    scene.AddGameObject(wall.GameObject);
//    scene.AddGameObject(go);
//    DrawCollider(scene, go.GetComponent<BoxCollider3D>());
//}



//private BoxCollider3D CreateCollider(MeshObjectData mesh, Vector3 position)
//{
//    var halfWidth = mesh.Vertices.Max(x => x.position.X);
//    var halfHeight = mesh.Vertices.Max(x => x.position.Y);

//    var width = halfWidth * 2;
//    var height = halfHeight * 2;
//    var depth = 5;

//    var a = position - new Vector3(0, halfHeight, halfWidth);
//    var b = a + new Vector3(depth, height, width);

//    return new BoxCollider3D(a, b)
//    {
//        IsStatic = true
//    };
//}


//private List<MeshComponent> DrawCollider(Scene scene, BoxCollider3D boxCollider)
//{
//    var list = new List<MeshComponent>();
//    var corners = boxCollider.BoundingBox.GetCorners();
//    foreach (var corner in corners)
//    {
//        var ball = _meshLoader.LoadFbx("assets\\models\\ball.fbx", _textureMaterialsProvider.DefaultTexture)[0];
//        ball.Transform.SetPosition(corner);
//        scene.AddGameObject(ball.GameObject);
//        list.Add(ball);
//    }
//    return list;
//}