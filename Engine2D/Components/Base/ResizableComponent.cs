using PhlegmaticOne.SharpTennis.Game.Engine2D.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Transform;

namespace PhlegmaticOne.SharpTennis.Game.Engine2D.Components.Base
{
    public class ResizableComponent : RectComponent
    {
        public ResizableComponent(RectTransform rectTransform)
        {
            RectTransform = rectTransform;
        }
    }
}
