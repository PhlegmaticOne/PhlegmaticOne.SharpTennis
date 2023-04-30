namespace PhlegmaticOne.SharpTennis.Game.Engine2D.Popups
{
    public abstract class PopupFactory<T> where T : PopupBase
    {
        private readonly T _popup;

        protected PopupFactory(T popup) => _popup = popup;

        public T CreatePopup()
        {
            var canvas = SetupPopup(_popup);
            _popup.SetupCanvas(canvas);
            return _popup;
        }

        public abstract Canvas SetupPopup(T popup);
    }
}
