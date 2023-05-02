using System.Collections.Generic;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Render;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components.Base;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Engine2D
{
    public class CanvasRenderer : Component, IRenderer
    {
        private readonly IElementsCreator _elementsCreator;
        private readonly IElementsRenderer _elementsRenderer;
        private readonly List<Canvas> _canvases;

        public CanvasRenderer(IElementsCreator elementsCreator, IElementsRenderer elementsRenderer)
        {
            _elementsCreator = elementsCreator;
            _elementsRenderer = elementsRenderer;
            _canvases = new List<Canvas>();
        }

        public void AddCanvas(Canvas canvas) => _canvases.Add(canvas);
        public void RemoveCanvas(Canvas canvas) => _canvases.Remove(canvas);

        public void ReinitializeElementsComponents(Canvas canvas)
        {
            foreach (var interfaceElement in canvas.GetElements())
            {
                UpdateUiElementComponents(interfaceElement);
            }
        }

        public void PreRender() { }

        public void BeginRender() => _elementsRenderer.BeginRender();

        public void Render()
        {
            foreach (var canvas in _canvases.ToArray())
            {
                foreach (var interfaceElement in canvas.GetElements())
                {
                    RenderComponent(interfaceElement);
                }
            }
        }

        public void RenderComponent(RectComponent rectComponent)
        {
            if (rectComponent.Enabled == false)
            {
                return;
            }

            var gameObject = rectComponent.GameObject;

            if (gameObject.TryGetComponent<ImageComponent>(out var image))
            {
                _elementsRenderer.RenderImage(image);
            }

            if (gameObject.TryGetComponent<TextComponent>(out var text))
            {
                _elementsRenderer.RenderText(text);
            }

            if (gameObject.TryGetComponent<Selectable>(out var selectable) && selectable.IsSelected)
            {
                _elementsRenderer.DrawRectangle(selectable.RectTransform, selectable.Brush, selectable.Stroke);
            }
        }

        public void EndRender() => _elementsRenderer.EndRender();

        public void Dispose() { }

        public void UpdateUiElementComponents(RectComponent interfaceElement)
        {
            if (interfaceElement.Enabled == false)
            {
                return;
            }

            var gameObject = interfaceElement.GameObject;

            if (gameObject.TryGetComponent<ImageComponent>(out var image))
            {
                image.Dispose();
                image.SetBitmap(_elementsCreator.CreateImage(image.FileName));
            }

            if (gameObject.TryGetComponent<TextComponent>(out var text))
            {
                text.Dispose();
                text.Brush = _elementsCreator.CreateBrush(text.BrushColor);
                text.TextFormat = _elementsCreator.CreateTextFormat(text.TextFormatData);
            }
        }
    }
}
