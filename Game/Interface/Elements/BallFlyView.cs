using System;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements
{
    public class BallFlyView : RectComponent
    {
        private readonly TextComponent _textComponent;

        public BallFlyView(TextComponent textComponent)
        {
            _textComponent = textComponent;
            RectTransform = textComponent.RectTransform;
        }

        public void UpdateTime(TimeSpan time)
        {
            _textComponent.Text = time.ToString();
        }
    }
}
