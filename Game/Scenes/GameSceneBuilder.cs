using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Base.Scenes;
using PhlegmaticOne.SharpTennis.Game.Common.Extensions;
using PhlegmaticOne.SharpTennis.Game.Common.Input;
using PhlegmaticOne.SharpTennis.Game.Common.Tween;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Game.Controllers;
using PhlegmaticOne.SharpTennis.Game.Game.Global;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Floor;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game;
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
        private readonly WinController _winController;
        private readonly BallBouncesController _ballBouncesController;
        private readonly GameDataProvider _gameDataProvider;
        private readonly GameListenerInitializer _gameListenerInitializer;
        private readonly GamePauseFacade _gameDynamicFacade;
        private readonly GameRestartFacade _gameRestartFacade;

        public GameSceneBuilder(IFactory<TennisTable> tennisTableFactory,
            IFactory<RacketBase, RacketFactoryData> racketFactory,
            IFactory<BallModel> ballFactory,
            IFactory<FloorModel> floorFactory,
            IFactory<SkyModel> skyFactory,
            InputController inputController,
            SceneProvider sceneProvider,
            WinController winController,
            BallBouncesController ballBouncesController,
            GameDataProvider gameDataProvider,
            GameListenerInitializer gameListenerInitializer,
            GamePauseFacade gameDynamicFacade,
            GameRestartFacade gameRestartFacade)
        {
            _tennisTableFactory = tennisTableFactory;
            _racketFactory = racketFactory;
            _ballFactory = ballFactory;
            _floorFactory = floorFactory;
            _skyFactory = skyFactory;
            _inputController = inputController;
            _sceneProvider = sceneProvider;
            _winController = winController;
            _ballBouncesController = ballBouncesController;
            _gameDataProvider = gameDataProvider;
            _gameListenerInitializer = gameListenerInitializer;
            _gameDynamicFacade = gameDynamicFacade;
            _gameRestartFacade = gameRestartFacade;
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
            var gameData = _gameDataProvider.GameData;
            var playerColor = gameData.PlayerColor == ColorType.Red ? Color.Red : Color.Black;
            var enemyColor = gameData.PlayerColor == ColorType.Red ? Color.Black : Color.Red;

            var sky = _skyFactory.Create(new Transform(rotation: new Vector3(0, -90, 0)));
            AddMeshableObject(scene, sky);

            var table = _tennisTableFactory.Create(Transform.EmptyIdentity);
            AddMeshableObject(scene, table);

            AddRacket(scene, new Transform(
                new Vector3(-70, 9, 0),
                new Vector3(90, -180, -90),
                new Vector3(1, 1, 1)), playerColor, Vector3.Right, true, table.TableTopPart);

            AddRacket(scene, new Transform(
                new Vector3(70, 9, 0),
                new Vector3(90, -180, -90),
                new Vector3(1, 1, 1)), enemyColor, Vector3.Left, false, table.TableTopPart);

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
            AddGameStateChecker(scene);
            AddDoTween(scene);
            AddPlayerRacketMoveController(scene);
            AddPhysicSystems(scene);
            AddWinController(scene);
            AddGameListenerInitializer(scene);

            InitializeGameFacades(scene);
        }

        private void InitializeGameFacades(Scene scene)
        {
            _gameDynamicFacade.Setup(
                scene.GetComponent<BallModel>(),
                scene.GetComponent<RacketMoveController>(),
                scene.GetComponent<EnemyRacket>());

            _gameRestartFacade.Setup(
                scene.GetComponent<BallModel>(),
                scene.GetComponent<PlayerRacket>(),
                scene.GetComponent<EnemyRacket>());
        }

        private void AddGameListenerInitializer(Scene scene)
        {
            var initializer = _gameListenerInitializer.WrapWithGameObject();
            initializer.Initialize();
            scene.AddGameObject(initializer.GameObject);
        }

        private void AddWinController(Scene scene)
        {
            var ballController = scene.GetComponent<BallBouncesController>();
            _winController.Setup(ballController);
            _winController.SetupPlayToScore(_gameDataProvider.GameData.PlayToScore);
            scene.AddGameObject(CreateGameObjectWithComponent("WinController", _winController));
        }


        private void AddGameStateChecker(Scene scene)
        {
            var player = scene.GetComponent<PlayerRacket>();
            var enemy = scene.GetComponent<EnemyRacket>();
            _ballBouncesController.SetupRackets(player, enemy);
            scene.AddGameObject(CreateGameObjectWithComponent("GameStateChecker", _ballBouncesController));
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
                TableNormal = tableTopPart.Normal,
                DifficultyType = _gameDataProvider.GameData.DifficultyType
            });

            AddMeshableObject(scene, racket);
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