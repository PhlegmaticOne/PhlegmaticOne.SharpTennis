using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Infrastructure;
using PhlegmaticOne.SharpTennis.Game.Common.Render;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Engine2D
{
    public class CanvasManager : BehaviorObject, IRenderer
    {
        private readonly InterfaceCursor _interfaceCursor;
        private readonly List<Canvas> _canvases;
        private CanvasRenderer _canvasRenderer;
        private CanvasScaler _canvasScaler;

        public CanvasManager(InterfaceCursor interfaceCursor)
        {
            _interfaceCursor = interfaceCursor;
            _canvases = new List<Canvas>();
        }

        public Canvas Current => _canvases.LastOrDefault();

        public override void Start()
        {
            _canvasRenderer = GameObject.GetComponent<CanvasRenderer>();
            _canvasScaler = GameObject.GetComponent<CanvasScaler>();
            GlobalEvents.ScreenResized += GameEventsOnScreenResized;
            GlobalEvents.MouseClicked += GameEventsOnMouseClicked;
            GlobalEvents.MouseMoved += GameEventsOnMouseMoved;
            GameEventsOnScreenResized(SizeF.Empty);

            foreach (var canvas in _canvases)
            {
                _canvasRenderer.AddCanvas(canvas);
            }
        }

        public void ChangeCursorEnabled(bool enabled)
        {
            _interfaceCursor.ChangeEnabled(enabled);
        }

        public void AddCanvas(Canvas canvas, bool setCursorEnabled = true)
        {
            ChangeCursorEnabled(setCursorEnabled);
            _canvases.Add(canvas);
            _canvasRenderer?.AddCanvas(canvas);
        }

        public void RemoveLast(bool setCursorEnabled = true)
        {
            var last = _canvases.LastOrDefault();
            _canvases.Remove(last);
            _canvasRenderer?.RemoveCanvas(last);
            last?.Dispose();
            ChangeCursorEnabled(setCursorEnabled);
        }

        private void GameEventsOnMouseMoved(Vector2 obj)
        {
            _interfaceCursor.Move(obj);
        }

        private void GameEventsOnMouseClicked()
        {
            var lastCanvas = _canvases.LastOrDefault();

            if (lastCanvas == null || lastCanvas.IsInteractable == false)
            {
                return;
            }

            _interfaceCursor.ClickOn(lastCanvas);
        }

        private void GameEventsOnScreenResized(SizeF obj)
        {
            Dispose();

            foreach (var canvas in _canvases)
            {
                _canvasRenderer.ReinitializeElementsComponents(canvas);
                _canvasScaler.RescaleElements(canvas);
            }

            _canvasRenderer.UpdateUiElementComponents(_interfaceCursor);
            _canvasScaler.ResizeElement(_interfaceCursor);
            _interfaceCursor.SetStartPosition(new Vector2(Screen.Width / 2, Screen.Height / 2));
        }


        public void Dispose()
        {
            _canvasRenderer?.Dispose();
            _interfaceCursor?.Dispose();
        }

        public override void OnDestroy()
        {
            foreach (var canvas in _canvases)
            {
                canvas.Dispose();
            }
        }

        public void PreRender()
        {
            _canvasRenderer.PreRender();
        }

        public void BeginRender()
        {
            _canvasRenderer.BeginRender();
        }

        public void Render()
        {
            _canvasRenderer.Render();
            _canvasRenderer.RenderComponent(_interfaceCursor.Image);
        }

        public void EndRender()
        {
            _canvasRenderer.EndRender();
        }
    }
}
