using System.Globalization;
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
            var enemyColorType = gameData.PlayerColor == ColorType.Red ? ColorType.Black : ColorType.Red;

            var sky = _skyFactory.Create(new Transform(rotation: new Vector3(0, -90, 0)));
            AddMeshableObject(scene, sky);

            var table = _tennisTableFactory.Create(Transform.EmptyIdentity);
            AddMeshableObject(scene, table);

            AddRacket(scene, new Transform(
                new Vector3(-70, 9, 0),
                new Vector3(90, -180, -90),
                new Vector3(1, 1, 1)), gameData.PlayerColor, Vector3.Right, true, table.TableTopPart,
                ParseColor(gameData.CustomColor));

            AddRacket(scene, new Transform(
                new Vector3(70, 9, 0),
                new Vector3(90, -180, -90),
                new Vector3(1, 1, 1)), enemyColorType, Vector3.Left, false, table.TableTopPart,
                Color.Black);

            var ball = _ballFactory.Create(new Transform(position: new Vector3(-50, 20, 20)));
            AddMeshableObject(scene, ball);

            AddFloors(scene);
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
            _winController.SetupGameData(_gameDataProvider.GameData);
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
                new RacketMoveController(playerRacket, scene.Camera, _inputController,
                    scene.GetComponent<BallModel>())));
        }


        private GameObject CreateGameObjectWithComponent<T>(string name, T component) where T : Component
        {
            var go = new GameObject(name);
            go.AddComponent(component);
            return go;
        }

        private void AddFloors(Scene scene)
        {
            var positions = new[]
            {
                new Vector3(50, -30, 0),
                new Vector3(50, -30, 90),
                new Vector3(50, -30, -90),
                new Vector3(50, -30, -180),
                new Vector3(50, -30, 180),
            };

            var line = 2;

            for (var i = -line; i <= line; i++)
            {
                foreach (var position in positions)
                {
                    var x = i * 89;
                    var floor = _floorFactory.Create(new Transform(
                        position + new Vector3(x, 0, 0),
                        new Vector3(0, -90, 90),
                        new Vector3(1, 1, 1)));
                    AddMeshableObject(scene, floor);
                }
            }
        }


        private void AddRacket(Scene scene, Transform transform, ColorType colorType, Vector3 normal, bool isPlayer,
            TableTopPart tableTopPart, Color custom)
        {
            var racket = _racketFactory.Create(transform, new RacketFactoryData
            {
                ColorType = colorType,
                IsPlayer = isPlayer,
                Normal = normal,
                TableHeight = tableTopPart.Size.X / 2,
                TableNormal = tableTopPart.Normal,
                DifficultyType = _gameDataProvider.GameData.DifficultyType,
                Custom = custom
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

        private static Color ParseColor(string hex)
        {
            var color = int.Parse(hex + "FF", NumberStyles.HexNumber);
            return Color.FromAbgr(color);
        }
    }
}