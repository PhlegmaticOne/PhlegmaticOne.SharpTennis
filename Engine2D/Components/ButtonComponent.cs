using System;
using System.Collections.Generic;
using System.Linq;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Base;

namespace PhlegmaticOne.SharpTennis.Game.Engine2D.Components
{
    public class ButtonComponent : RectComponent
    {
        public ButtonComponent(ImageComponent imageComponent, TextComponent textComponent)
        {
            RectTransform = imageComponent.RectTransform;
            textComponent.RectTransform = imageComponent.RectTransform;
            OnClick = new List<Action>();
        }

        public ButtonComponent(ImageComponent imageComponent)
        {
            RectTransform = imageComponent.RectTransform;
        }

        public List<Action> OnClick { get; }


        public override void HandleClick()
        {
            foreach (var action in OnClick.ToList())
            {
                action?.Invoke();
            }
        }

        public override void Dispose()
        {
            OnClick.Clear();
        }
    }
}
