using System;
using PhlegmaticOne.SharpTennis.Game.Common.Infrastructure;
using PhlegmaticOne.SharpTennis.Game.Common.Input;
using PhlegmaticOne.SharpTennis.Game.Common.Render;
using PhlegmaticOne.SharpTennis.Game.Engine2D;
using PhlegmaticOne.SharpTennis.Game.Engine2D.DirectX;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Engine3D.DirectX;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Rigid;
using PhlegmaticOne.SharpTennis.Game.Game.Controllers;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Racket;
using PhlegmaticOne.SharpTennis.Game.Game.Scenes;
using SharpDX;
using SharpDX.DirectInput;
using SharpDX.DXGI;
using SharpDX.Windows;
using Scene = PhlegmaticOne.SharpTennis.Game.Common.Base.Scene;
using Screen = PhlegmaticOne.SharpTennis.Game.Common.Infrastructure.Screen;

namespace PhlegmaticOne.SharpTennis.Game.Game
{
    public class GameRunner : IDisposable
    {
        private readonly RenderForm _renderForm;
        private readonly DirectX3DGraphics _directX3DGraphics;
        private readonly DirectX2DGraphics _directX2DGraphics;
        private readonly InputController _inputController;

        private bool _firstRun = true;

        private readonly RenderSequence _renderSequence;
        private readonly CanvasManager _canvasManager;
        private readonly MeshRenderer _meshRenderer;
        private readonly RigidBodiesSystem _rigidBodiesSystem;
        private readonly CollidingSystem _collisionSystem;
        private RacketMoveController _racketMoveController;

        public GameRunner(RenderForm renderForm)
        {
            _renderForm = renderForm;
            _directX3DGraphics = new DirectX3DGraphics(_renderForm);
            _directX2DGraphics = new DirectX2DGraphics();
            _inputController = new InputController(_renderForm);

            _meshRenderer = new MeshRenderer(_directX3DGraphics, new MeshRendererData(
                new ShaderInfo("vertex.hlsl", "vertexShader", "vs_5_0"),
                new ShaderInfo("pixel.hlsl", "pixelShader", "ps_5_0")));

            //var canvasManager = new CanvasManagerFactory(_directX2DGraphics, _directX2DGraphics).CreateCanvasManager();
            //var factory = new MenuCanvasFactory(new MenuCanvasViewModel
            //{
            //    ExitButtonCommand = new ExitGameCommand(_renderForm),
            //    PlayButtonCommand = new StartGameCommand(canvasManager, 
            //        new GameSceneBuilder(new TextureMaterialsProvider(),
            //            new MeshLoader(_directX3DGraphics, meshRenderer)), this)
            //});
            //canvasManager.AddCanvas(factory.CreateCanvas());

            //_canvasManager = canvasManager;
            _rigidBodiesSystem = new RigidBodiesSystem();
            _collisionSystem = new CollidingSystem();


            _renderSequence = new RenderSequence(new IRenderer[]
            {
                _meshRenderer,
                //canvasManager
            });
            _renderForm.UserResized += RenderFormResizedCallback;
        }


        public void RenderFormResizedCallback(object sender, EventArgs args)
        {
            Screen.Initialize(_renderForm.ClientSize);
            if (Scene.Current != null)
            {
                var camera = Scene.Current.Camera;
                camera.Aspect = Screen.Width / Screen.Height;
            }
            //_canvasManager.Dispose();
            _directX2DGraphics.DisposeOnResizing();
            _directX3DGraphics.Resize();
            _directX2DGraphics.Resize(_directX3DGraphics.BackBuffer.QueryInterface<Surface>());
            GameEvents.OnScreenResized(Screen.Size);
        }


        public void RenderLoopCallback()
        {
            if (_firstRun)
            {
                var scene = new GameSceneBuilder(new TextureMaterialsProvider(),
                        new MeshLoader(_directX3DGraphics, _meshRenderer.PointSampler))
                    .BuildScene();

                var racket = scene.GetComponent<Racket>();
                _racketMoveController = new RacketMoveController(racket, scene.Camera, _inputController);
                scene.Start();
                RenderFormResizedCallback(this, EventArgs.Empty);
                //_canvasManager.Start();
                _firstRun = false;
            }

            Time.Update();

            MoveCamera();
            _racketMoveController.UpdateBehavior();
            _collisionSystem.UpdateBehavior();
            _rigidBodiesSystem.UpdateBehavior();

            GameEvents.OnMouseMoved(new Vector2(_inputController.MouseRelativePositionX, _inputController.MouseRelativePositionY));

            if (_inputController.MouseLeft)
            {
                GameEvents.OnMouseClicked();
            }

            _renderSequence.UpdateBehavior();
        }

        private void MoveCamera()
        {
            var camera = Scene.Current.Camera;
            _inputController.UpdateKeyboardState();
            _inputController.UpdateMouseState();

            if (_inputController.IsPressed(Key.S))
            {
                camera.Transform.Move(Vector3.UnitX / -5);
            }

            if (_inputController.IsPressed(Key.W))
            {
                camera.Transform.Move(Vector3.UnitX / 5);
            }

            if (_inputController.IsPressed(Key.Q))
            {
                camera.Transform.Move(Vector3.UnitY / 5);
            }

            if (_inputController.IsPressed(Key.E))
            {
                camera.Transform.Move(Vector3.UnitY / -5);
            }

            if (_inputController.IsPressed(Key.A))
            {
                camera.Transform.Move(Vector3.UnitZ / 5);
            }

            if (_inputController.IsPressed(Key.D))
            {
                camera.Transform.Move(Vector3.UnitZ / -5);
            }

            //if (_inputController.MouseUpdated)
            //{
            //    var deltaAngle = camera.FovY / _renderForm.ClientSize.Height;

            //    camera.Transform.Rotate(new Vector3(
            //        deltaAngle * _inputController.MouseRelativePositionX * 40,
            //        deltaAngle * _inputController.MouseRelativePositionY * 40, 0));
            //}
        }

        public void Run() => RenderLoop.Run(_renderForm, RenderLoopCallback);

        public void Dispose()
        {
            _inputController.Dispose();
            _directX3DGraphics.Dispose();
        }
    }
}
