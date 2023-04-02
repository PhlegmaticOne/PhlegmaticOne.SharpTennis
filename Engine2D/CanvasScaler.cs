using System;
using System.Drawing;
using System.Linq;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Infrastructure;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Transform;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Engine2D
{
    public class CanvasScaler : Component
    {
        private readonly SizeF _baseSize;

        public CanvasScaler(SizeF baseSize)
        {
            _baseSize = baseSize;
        }

        public void RescaleElements(Canvas canvas)
        {
            foreach (var interfaceElement in canvas.GetElements().Where(x => x is IResizable))
            {
                ResizeElement(interfaceElement);
            }
        }

        public void ResizeElement(RectComponent element)
        {
            if (element.Enabled == false)
            {
                return;
            }

            var transform = element.RectTransform;
            var scaleFactor = Math.Min(Screen.Width / _baseSize.Width, Screen.Height / _baseSize.Height);

            var scaleVector = transform.Anchor == Anchor.Stretch
                ? new Vector2(Screen.Width / _baseSize.Width, Screen.Height / _baseSize.Height)
                : new Vector2(scaleFactor, scaleFactor);

            transform.SetScale(Vector3.One * (Vector3)scaleVector);
            transform.Position2 = GenerateMaxPosition(transform.Anchor) + scaleFactor * transform.Offset;
        }


        private static Vector2 GenerateMaxPosition(Anchor anchor)
        {
            switch (anchor)
            {
                case Anchor.Center:
                    return new Vector2(Screen.Width / 2, Screen.Height / 2);
                case Anchor.Stretch:
                    return new Vector2(Screen.Width / 2, Screen.Height / 2);
                case Anchor.Left:
                    return new Vector2(-Screen.Width, 0f);
                case Anchor.Right:
                    return new Vector2(Screen.Width, 0f);
                case Anchor.Top:
                    return new Vector2(0, -Screen.Height);
                case Anchor.Bottom:
                    return new Vector2(0f, Screen.Height);
                case Anchor.TopRight:
                    return new Vector2(Screen.Width, -Screen.Height);
                case Anchor.BottomLeft:
                    return new Vector2(-Screen.Width, Screen.Height);
                case Anchor.BottomRight:
                    return new Vector2(Screen.Width, Screen.Height);
                case Anchor.TopLeft:
                    return new Vector2(-Screen.Width, -Screen.Height);
            }

            return Vector2.Zero;
        }
    }
}
