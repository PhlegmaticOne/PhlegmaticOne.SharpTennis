using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Transform;
using SharpDX.Direct2D1;

namespace PhlegmaticOne.SharpTennis.Game.Engine2D.Base
{
    public interface IElementsRenderer
    {
        void BeginRender();
        void EndRender();
        void RenderText(TextComponent textComponent);
        void DrawRectangle(RectTransform rectTransform, Brush brush, float stroke);
        void RenderImage(ImageComponent imageComponent);
    }
}
