using System;

namespace PhlegmaticOne.SharpTennis.Game.Common.Base.Scenes
{
    public class SceneProvider
    {
        private Scene _newScene;
        public Scene Scene { get; private set; }

        public event Action<Scene, Scene> SceneChanged; 

        public void ChangeScene(Scene scene)
        {
            _newScene = scene;
            Scene?.OnDestroy();
            Scene = scene;
            Scene.Start();
        }

        public void UpdateScene()
        {
            Scene?.UpdateBehavior();
        }
    }
}
