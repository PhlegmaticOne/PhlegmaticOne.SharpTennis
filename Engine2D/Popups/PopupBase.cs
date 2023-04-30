using PhlegmaticOne.SharpTennis.Game.Common.Base;

namespace PhlegmaticOne.SharpTennis.Game.Engine2D.Popups
{
    public abstract class PopupBase : BehaviorObject
    {
        public Canvas Canvas { get; private set; }
        public void SetupCanvas(Canvas canvas) => Canvas = canvas;
        public void Show() => OnShow();
        public void Close() => OnClose();
        protected virtual void OnShow() { }
        protected virtual void OnClose() { }
    }
}
