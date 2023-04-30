using System.Linq;
using PhlegmaticOne.SharpTennis.Game.Common.Infrastructure;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Base;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Engine2D.Components
{
    public class InterfaceCursor : RectComponent
    {
        private Vector2 _startPosition;
        public ImageComponent Image { get; }

        public override void Dispose()
        {
            var image = Image;
            Utilities.Dispose(ref image);
        }

        public void ChangeEnabled(bool enabled)
        {
            Enabled = enabled;
            Image.Enabled = enabled;
        }

        public InterfaceCursor(ImageComponent imageComponent)
        {
            RectTransform = imageComponent.RectTransform;
            Image = imageComponent;
            Reset();
        }

        public void SetStartPosition(Vector2 startPosition)
        {
            _startPosition = startPosition;
        }

        public void Move(Vector2 relativeMove)
        {
            RectTransform.Move((Vector3)relativeMove);

            var size = RectTransform.CalculateHalfSize();
            var position = RectTransform.Position2;

            if (position.X > Screen.Width - size.X || position.X < size.X ||
                position.Y > Screen.Height - size.Y || position.Y < size.Y)
            {
                RectTransform.Move(-(Vector3)relativeMove);
            }
        }

        public void Reset() => RectTransform.SetPosition((Vector3)_startPosition);

        public void ClickOn(Canvas canvas)
        {
            foreach (var rectComponent in canvas.GetElements().ToList())
            {
                if (rectComponent.RectTransform.RectBounds.Contains(RectTransform.Position2))
                {
                    rectComponent.HandleClick();
                }
            }
        }
    }
}
