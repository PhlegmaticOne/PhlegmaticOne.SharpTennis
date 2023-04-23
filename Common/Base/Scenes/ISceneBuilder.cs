using PhlegmaticOne.SharpTennis.Game.Engine2D;

namespace PhlegmaticOne.SharpTennis.Game.Common.Base.Scenes
{
    public interface ISceneBuilder
    {
        Scene BuildScene();
        void SetupSceneCanvas(Scene scene, Canvas sceneCanvas);
    }
}
