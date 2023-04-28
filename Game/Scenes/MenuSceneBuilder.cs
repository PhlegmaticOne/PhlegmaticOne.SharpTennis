using PhlegmaticOne.SharpTennis.Game.Common.Base.Scenes;
using PhlegmaticOne.SharpTennis.Game.Engine2D;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Base;

namespace PhlegmaticOne.SharpTennis.Game.Game.Scenes
{
    public class MenuSceneBuilder : ISceneBuilder
    {
        private readonly CanvasManager _canvasManager;
        private readonly ICanvasFactory _canvasFactory;

        public MenuSceneBuilder(CanvasManager canvasManager, ICanvasFactory canvasFactory)
        {
            _canvasManager = canvasManager;
            _canvasFactory = canvasFactory;
        }

        public Scene BuildScene()
        {
            var scene = new Scene(_canvasManager.GameObject);
            var canvas = _canvasFactory.CreateCanvasForScene(scene);
            _canvasManager.AddCanvas(canvas);
            return scene;
        }
    }
}
