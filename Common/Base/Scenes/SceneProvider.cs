namespace PhlegmaticOne.SharpTennis.Game.Common.Base.Scenes
{
    public class SceneProvider
    {
        private Scene _scene;
        public Scene Scene => _scene;

        public void ChangeScene(Scene scene)
        {
            _scene?.OnDestroy();
            _scene = scene;
            _scene.Start();
        }

        public void UpdateScene() => _scene?.UpdateBehavior();
    }
}
