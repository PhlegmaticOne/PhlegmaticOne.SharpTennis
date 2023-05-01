using PhlegmaticOne.SharpTennis.Game.Common.Base.Scenes;
using PhlegmaticOne.SharpTennis.Game.Common.Extensions;
using PhlegmaticOne.SharpTennis.Game.Common.Input;
using PhlegmaticOne.SharpTennis.Game.Engine2D;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Game.Global;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Menu;

namespace PhlegmaticOne.SharpTennis.Game.Game.Scenes
{
    public class MenuSceneBuilder : ISceneBuilder
    {
        private readonly CanvasManager _canvasManager;
        private readonly PopupSystem _popupSystem;
        private readonly MenuListenerInitializer _menuListenerInitializer;
        private readonly GlobalInputListener _globalInputListener;

        public MenuSceneBuilder(CanvasManager canvasManager, 
            PopupSystem popupSystem, 
            MenuListenerInitializer menuListenerInitializer,
            GlobalInputListener globalInputListener)
        {
            _canvasManager = canvasManager;
            _popupSystem = popupSystem;
            _menuListenerInitializer = menuListenerInitializer;
            _globalInputListener = globalInputListener;
        }

        public Scene BuildScene()
        {
            _menuListenerInitializer.Initialize();
            var scene = new Scene(_canvasManager.GameObject,
                _globalInputListener.GameObject,
                _menuListenerInitializer.WrapWithGameObject().GameObject);
            _canvasManager.Start();
            _popupSystem.SpawnPopup<MenuPopup>();
            return scene;
        }
    }
}
