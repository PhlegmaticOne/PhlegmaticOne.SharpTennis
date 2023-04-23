using System.Collections.Generic;
using System.Linq;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Base.Scenes;

namespace PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders
{
    public class CollidingSystem : BehaviorObject
    {
        private readonly SceneProvider _sceneProvider;
        private readonly List<CollisionContainer> _collisions;

        public CollidingSystem(SceneProvider sceneProvider)
        {
            _sceneProvider = sceneProvider;
            _collisions = new List<CollisionContainer>();
        }

        protected override void Update()
        {
            TryAddNewCollisions();
            TryRemoveExistingCollisions();
        }

        private void TryAddNewCollisions()
        {
            var allColliders = _sceneProvider.Scene.GetComponents<Collider>().ToList();

            for (var i = 0; i < allColliders.Count; i++)
            {
                for (var j = i + 1; j < allColliders.Count; j++)
                {
                    var a = allColliders[i];
                    var b = allColliders[j];

                    if (a.Intersects(b))
                    {
                        var container = new CollisionContainer(a, b);
                        if (_collisions.Contains(container) == false)
                        {
                            _collisions.Add(container);
                            container.CollideObjects();
                        }
                    }
                }
            }
        }

        private void TryRemoveExistingCollisions()
        {
            for (var i = _collisions.Count - 1; i >= 0; i--)
            {
                var collision = _collisions[i];

                if (collision.DontCollideAnymore())
                {
                    _collisions.Remove(collision);
                }
            }
        }
    }
}
