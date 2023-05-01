namespace PhlegmaticOne.SharpTennis.Game.Common.Base.Scenes
{
    public class SceneProvider
    {
        public Scene Scene { get; private set; }

        public void ChangeScene(Scene scene)
        {
            Scene?.OnDestroy();
            Scene = scene;
            Scene.Start();
        }

        public void UpdateScene() => Scene?.UpdateBehavior();
    }
}
