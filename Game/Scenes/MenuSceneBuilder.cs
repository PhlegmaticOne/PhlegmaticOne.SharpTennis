using PhlegmaticOne.SharpTennis.Game.Common.Base.Scenes;
using PhlegmaticOne.SharpTennis.Game.Engine2D;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Menu;

namespace PhlegmaticOne.SharpTennis.Game.Game.Scenes
{
    public class MenuSceneBuilder : ISceneBuilder
    {
        private readonly CanvasManager _canvasManager;
        private readonly PopupSystem _popupSystem;

        public MenuSceneBuilder(CanvasManager canvasManager, PopupSystem popupSystem)
        {
            _canvasManager = canvasManager;
            _popupSystem = popupSystem;
        }

        public Scene BuildScene()
        {
            var scene = new Scene(_canvasManager.GameObject);
            _canvasManager.Start();
            _popupSystem.SpawnPopup<MenuPopup>();
            return scene;
        }
    }
}
