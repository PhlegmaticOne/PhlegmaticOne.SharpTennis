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
            var canvas = _canvasFactory.CreateCanvas();
            _canvasManager.AddCanvas(canvas);
            return new Scene(_canvasManager.GameObject);
        }

        public void SetupSceneCanvas(Scene scene, Canvas sceneCanvas)
        {
            
        }
    }
}
