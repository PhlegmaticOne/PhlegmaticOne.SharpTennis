using PhlegmaticOne.SharpTennis.Game.Engine2D.Base;
using SharpDX.Direct2D1;

namespace PhlegmaticOne.SharpTennis.Game.Engine2D.Components.Base
{
    public class Selectable : RectComponent
    {
        public bool DeselectOnMisClick { get; set; }
        public bool IsSelected { get; private set; }
        public Brush Brush { get; protected set; }
        public float Stroke { get; set; } = 5;

        public void Select()
        {
            IsSelected = true;
            OnSelect();
        }

        public void Deselect()
        {
            IsSelected = false;
        }

        protected virtual void OnSelect() { }
    }
}
