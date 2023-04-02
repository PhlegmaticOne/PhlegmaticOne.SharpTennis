using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;

namespace PhlegmaticOne.SharpTennis.Game.Engine2D.Base
{
    public interface IElementsRenderer
    {
        void BeginRender();
        void EndRender();
        void RenderText(TextComponent textComponent);
        void RenderImage(ImageComponent imageComponent);
    }
}
