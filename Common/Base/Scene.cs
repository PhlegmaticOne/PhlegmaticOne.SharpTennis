using System.Collections.Generic;
using System.Linq;
using PhlegmaticOne.SharpTennis.Game.Engine3D;

namespace PhlegmaticOne.SharpTennis.Game.Common.Base
{
    public class Scene : BehaviorObject
    {
        private readonly List<GameObject> _gameObjects;

        public static Scene Current { get; private set; }
        public Camera Camera { get; private set; }

        public Scene() : this(new List<GameObject>()) { }

        public Scene(IEnumerable<GameObject> gameObjects)
        {
            _gameObjects = new List<GameObject>();
            _gameObjects.AddRange(gameObjects);
            Current = this;
        }

        public override void Start()
        {
            Camera = GetComponent<Camera>();
        }

        public void AddGameObject(GameObject gameObject)
        {
            _gameObjects.Add(gameObject);
        }

        public void AddGameObjects(IEnumerable<GameObject> gameObjects)
        {
            _gameObjects.AddRange(gameObjects);
        }

        public T GetComponent<T>() where T : Component => 
            _gameObjects.First(x => x.HasComponent<T>()).GetComponent<T>();

        public IEnumerable<T> GetComponents<T>() where T : Component => 
            _gameObjects.Where(x => x.HasComponent<T>()).Select(x => x.GetComponent<T>());

        public override void OnDestroy()
        {
            foreach (var gameObject in _gameObjects)
            {
                gameObject.Destroy();
            }

            _gameObjects.Clear();
        }
    }
}
