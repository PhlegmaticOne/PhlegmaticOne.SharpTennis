using System.Collections.Generic;
using System.Linq;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Base;

namespace PhlegmaticOne.SharpTennis.Game.Engine2D
{
    public class Canvas : BehaviorObject
    {
        private readonly List<RectComponent> _elements;

        public static Canvas Create(string name, params GameObject[] objects)
        {
            var gameObject = new GameObject();
            var elements = objects.SelectMany(x => x.GetComponents<RectComponent>());
            var canvas = new Canvas(name, elements);
            gameObject.AddComponent(canvas);
            return canvas;
        }

        public Canvas(string name, IEnumerable<RectComponent> elements)
        {
            Name = name;
            _elements = elements.ToList();
        }

        public bool IsInteractable { get; set; } = true;
        public string Name { get; set; }

        public IEnumerable<RectComponent> GetElements() => _elements.Where(x => x.Enabled);

        public void AddElement(RectComponent element) => _elements.Add(element);

        public void Dispose()
        {
            foreach (var rectComponent in _elements)
            {
                rectComponent.Dispose();
            }
        }
    }
}
