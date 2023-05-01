using System;
using System.Collections.Generic;
using System.Linq;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Base;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Engine2D.Components
{
    public class SelectablePanel<T> : RectComponent
    {
        private readonly List<SelectableComponent<T>> _selectableComponents;

        public T Value { get; private set; }
        public IReadOnlyList<SelectableComponent<T>> SelectableComponents => _selectableComponents;
        public IEnumerable<GameObject> SelectableObjects => _selectableComponents.Select(x => x.GameObject);

        private SelectablePanel(IEnumerable<SelectableComponent<T>> selectableComponents)
        {
            _selectableComponents = new List<SelectableComponent<T>>(selectableComponents);

            foreach (var selectableComponent in _selectableComponents)
            {
                selectableComponent.Selected += SelectableComponentOnSelected;    
            }
        }

        public static SelectablePanel<T> Create(IEnumerable<SelectableComponent<T>> selectableComponents)
        {
            var go = new GameObject();
            var panel = new SelectablePanel<T>(selectableComponents);
            go.AddComponent(panel, false);
            return panel;
        }

        private void SelectableComponentOnSelected(SelectableComponent<T> arg2)
        {
            Value = arg2.Value;

            foreach (var selectableComponent in _selectableComponents)
            {
                if (selectableComponent != arg2)
                {
                    selectableComponent.Deselect();
                }
            }
        }

        public void SetItemsOffset(float itemWidth, Vector2 offset, float spacing)
        {
            if (_selectableComponents.Count == 1)
            {
                _selectableComponents.First().RectTransform.Offset = offset;
            }

            if (_selectableComponents.Count % 2 == 1)
            {
                for (var i = 0; i < _selectableComponents.Count; i++)
                {
                    var item = _selectableComponents[i];
                    var c = i < _selectableComponents.Count / 2 ? -1 : 1;
                    var n = Math.Abs(i - _selectableComponents.Count / 2);
                    var space = c * n * (spacing + itemWidth);
                    var off = offset + new Vector2(space, 0);
                    item.RectTransform.Offset = off;
                }

                return;
            }

            var baseOff = spacing / 2;

            for (var i = 0; i < _selectableComponents.Count; i++)
            {
                var item = _selectableComponents[i];
                var c = i < _selectableComponents.Count / 2 ? -1 : 1;
                var n = Math.Abs(i - _selectableComponents.Count / 2);

                if (c == 1)
                {
                    n++;
                }

                var space = c * (baseOff + (2.0f * n - 1) / 2 * itemWidth + (n - 1) * spacing);
                var off = offset + new Vector2(space, 0);
                item.RectTransform.Offset = off;
            }
        }
    }
}
