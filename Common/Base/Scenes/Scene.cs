using System.Collections.Generic;
using System.Linq;
using PhlegmaticOne.SharpTennis.Game.Engine3D;

namespace PhlegmaticOne.SharpTennis.Game.Common.Base.Scenes
{
    public class Scene : BehaviorObject
    {
        private static readonly List<BehaviorObject> _dontDestroyOnLoad = new List<BehaviorObject>();
        private static readonly List<GameObject> _dontDestroyOnLoadObjects = new List<GameObject>();

        private readonly List<GameObject> _gameObjects;

        public Camera Camera { get; set; }

        public IReadOnlyList<GameObject> GameObjects => _gameObjects;

        public Scene() : this(new List<GameObject>()) { }

        public Scene(params GameObject[] gameObjects)
        {
            _gameObjects = new List<GameObject>();
            _gameObjects.AddRange(gameObjects);
        }

        public Scene(IEnumerable<GameObject> gameObjects)
        {
            _gameObjects = new List<GameObject>(gameObjects);
            var donDestroy = _gameObjects.Where(x => x.DestroyOnLoad == false);

            _dontDestroyOnLoad.AddRange(_gameObjects
                .SelectMany(x => x.GetComponents<BehaviorObject>()));
            _dontDestroyOnLoadObjects.AddRange(donDestroy);
        }

        public override void Start()
        {
            foreach (var behaviorObject in GetBehaviors())
            {
                behaviorObject.Start();
            }
        }

        protected override void Update()
        {
            foreach (var behavior in _dontDestroyOnLoad)
            {
                behavior.UpdateBehavior();
            }

            foreach (var behaviorObject in GetBehaviors())
            {
                behaviorObject.UpdateBehavior();
            }
        }

        public void AddGameObject(GameObject gameObject)
        {
            if (gameObject.DestroyOnLoad == false)
            {
                if (_dontDestroyOnLoadObjects.Contains(gameObject) == false)
                {
                    _dontDestroyOnLoad.AddRange(gameObject.GetComponents<BehaviorObject>());
                    _dontDestroyOnLoadObjects.Add(gameObject);
                }

                return;
            }

            _gameObjects.Add(gameObject);
        }

        public T GetComponent<T>() where T : Component => 
            _gameObjects.First(x => x.HasComponent<T>()).GetComponent<T>();

        public IEnumerable<T> GetComponents<T>() where T : Component => 
            _gameObjects.Where(x => x.HasComponent<T>()).Select(x => x.GetComponent<T>());

        public override void OnDestroy()
        {
            foreach (var gameObject in _gameObjects)
            {
                if (gameObject.DestroyOnLoad == false)
                {
                    continue;
                }
                gameObject.Destroy();
            }

            _gameObjects.Clear();
        }

        private IEnumerable<BehaviorObject> GetBehaviors() =>
            _gameObjects.SelectMany(x => x.GetComponents<BehaviorObject>());
    }
}
