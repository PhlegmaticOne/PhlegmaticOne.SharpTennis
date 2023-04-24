using System;
using System.ComponentModel;
using PhlegmaticOne.SharpTennis.Game.Common.Base.Scenes;
using PhlegmaticOne.SharpTennis.Game.Common.Infrastructure;
using PhlegmaticOne.SharpTennis.Game.Common.Input;
using PhlegmaticOne.SharpTennis.Game.Common.Render;
using PhlegmaticOne.SharpTennis.Game.Engine2D.DirectX;
using PhlegmaticOne.SharpTennis.Game.Engine3D.DirectX;
using SharpDX;
using SharpDX.DXGI;
using SharpDX.Windows;
using Screen = PhlegmaticOne.SharpTennis.Game.Common.Infrastructure.Screen;

namespace PhlegmaticOne.SharpTennis.Game.Game
{
    public class GameRunner<TScenes> : IDisposable where TScenes : IGameScenes
    {
        private readonly RenderForm _renderForm;
        private readonly DirectX3DGraphics _directX3DGraphics;
        private readonly DirectX2DGraphics _directX2DGraphics;
        private readonly InputController _inputController;
        private readonly SceneProvider _sceneProvider;
        private readonly ISceneBuilderFactory<TScenes> _sceneBuilderFactory;
        private readonly RenderSequence _renderSequence;

        public GameRunner(RenderForm renderForm,
            DirectX2DGraphics directX2DGraphics,
            DirectX3DGraphics directX3DGraphics,
            RenderSequence renderSequence,
            InputController inputController,
            SceneProvider sceneProvider,
            ISceneBuilderFactory<TScenes> startSceneBuilder)
        {
            _renderForm = renderForm;
            _renderSequence = renderSequence;
            _inputController = inputController;
            _sceneProvider = sceneProvider;
            _sceneBuilderFactory = startSceneBuilder;
            _directX3DGraphics = directX3DGraphics;
            _directX2DGraphics = directX2DGraphics;
            _renderForm.UserResized += RenderFormResizedCallback;
            _renderForm.Closing += RenderFormOnClosing;
        }

        public void Run() => RenderLoop.Run(_renderForm, RenderLoopCallback);

        public void Dispose()
        {
            _inputController.Dispose();
            _directX2DGraphics.Dispose();
            _directX3DGraphics.Dispose();
            _renderForm.UserResized -= RenderFormResizedCallback;
            _renderForm.Closing -= RenderFormOnClosing;
        }

        public void ForceResize() => RenderFormResizedCallback(null, EventArgs.Empty);

        private void RenderLoopCallback()
        {
            if (GlobalVariables.IsDisposed)
            {
                return;
            }

            if (GlobalVariables.IsFirstRun)
            {
                StartGameFromDefaultScene();
                GlobalVariables.IsFirstRun = false;
            }

            UpdateGame();
        }

        private void UpdateGame()
        {
            Time.Update();
            UpdateInput();
            RaiseGlobalEvents();
            _sceneProvider.UpdateScene();
            _renderSequence.UpdateBehavior();
        }

        private void UpdateInput()
        {
            _inputController.UpdateKeyboardState();
            _inputController.UpdateMouseState();
        }

        private void RaiseGlobalEvents()
        {
            GlobalEvents.OnMouseMoved(new Vector2(_inputController.MouseRelativePositionX, _inputController.MouseRelativePositionY));

            if (_inputController.MouseLeft)
            {
                GlobalEvents.OnMouseClicked();
            }
        }


        private void RenderFormOnClosing(object sender, CancelEventArgs e)
        {
            GlobalVariables.IsDisposed = true;
            _sceneProvider.Scene?.OnDestroy();
        }

        private void RenderFormResizedCallback(object sender, EventArgs args)
        {
            Screen.Initialize(_renderForm.ClientSize);
            _directX2DGraphics.DisposeOnResizing();
            _directX3DGraphics.Resize();
            _directX2DGraphics.Resize(_directX3DGraphics.BackBuffer.QueryInterface<Surface>());
            GlobalEvents.OnScreenResized(Screen.Size);
            ResizeSceneCamera();
        }

        private void ResizeSceneCamera()
        {
            var camera = _sceneProvider.Scene?.Camera;

            if (camera != null)
            {
                camera.Aspect = Screen.Width / Screen.Height;
            }
        }

        private void StartGameFromDefaultScene()
        {
            var sceneBuilder = _sceneBuilderFactory.CreateSceneBuilder(_sceneBuilderFactory.Scenes.Default);
            _sceneProvider.ChangeScene(sceneBuilder.BuildScene());
            ForceResize();
        }
    }
}
