using System;
using System.Collections.Generic;
using System.Linq;

namespace PhlegmaticOne.SharpTennis.Game.Common.Base
{
    public sealed class GameObject : BaseObject
    {
        private readonly List<Component> _components;

        public GameObject()
        {
            _components = new List<Component>();
            Transform = Transform.Identity(this);
        }

        public Transform Transform { get; }

        public void AddComponent(Component component, bool setDefaultTransform = true)
        {
            component.GameObject = this;

            if (setDefaultTransform)
            {
                component.Transform = Transform;
            }

            _components.Add(component);
        }

        public bool HasComponent<T>() where T : Component => 
            _components.Any(x => x is T);


        public T GetComponent<T>() where T : Component => 
            (T)_components.FirstOrDefault(x => x is T);

        public IEnumerable<T> GetComponents<T>() where T : Component => _components.OfType<T>();

        public bool TryGetComponent<T>(out T component) where T : Component
        {
            component = GetComponent<T>();
            return component != null;
        }

        public void Destroy()
        {
            foreach (var component in _components)
            {
                component.GameObject = null;
                component.Transform = null;

                if (component is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            _components.Clear();
        }
    }
}
