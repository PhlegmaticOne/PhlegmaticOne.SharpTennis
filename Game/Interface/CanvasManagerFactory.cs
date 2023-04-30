using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Transform;
using SharpDX;
using System.Drawing;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface
{
    public class CanvasManagerFactory
    {
        private readonly IElementsRenderer _elementsRenderer;
        private readonly IElementsCreator _elementsCreator;

        public CanvasManagerFactory(IElementsRenderer elementsRenderer, IElementsCreator elementsCreator)
        {
            _elementsRenderer = elementsRenderer;
            _elementsCreator = elementsCreator;
        }
        
        public CanvasManager CreateCanvasManager()
        {
            var cursor = new GameObject();
            var cursorImage = ImageComponent.Create(@"assets\textures\ui\cursor.png", Vector2.Zero, Anchor.Center);
            cursorImage.RectTransform.IsPivot = true;
            cursorImage.RectTransform.SetRotation(new Vector3(0, 0, -40));
            cursor.AddComponent(cursorImage, false);
            cursor.AddComponent(new InterfaceCursor(cursorImage), false);

            var manager = new GameObject();
            var canvasManager = new CanvasManager(cursor.GetComponent<InterfaceCursor>());
            manager.AddComponent(canvasManager);
            manager.AddComponent(new CanvasScaler(new SizeF(1920, 1080)));
            manager.AddComponent(new CanvasRenderer(_elementsCreator, _elementsRenderer));

            canvasManager.Initialize();
            return canvasManager;
        }
    }
}
