using System;
using System.Drawing;
using SharpDX;
using SharpDX.Mathematics.Interop;
using RectangleF = SharpDX.RectangleF;

namespace PhlegmaticOne.SharpTennis.Game.Engine2D.Transform
{
    public class RectTransform : Common.Base.Transform
    {
        public bool IsPivot { get; set; } = false;
        public Vector2 Offset { get; set; }
        public Vector2 Pivot { get; set; }
        public SizeF Size { get; set; }
        public Anchor Anchor { get; set; }
        public Vector2 Position2
        { 
            get => (Vector2)Position;
            set => SetPosition((Vector3)value);
        }

        public static RectTransform RectIdentity => new RectTransform(Vector2.Zero, Vector2.Zero, SizeF.Empty, Anchor.Center); 

        public RawRectangleF RawBounds => new RawRectangleF(0, 0, Size.Width, Size.Height);

        public RectangleF RectBounds
        {
            get
            {
                var size = CalculateHalfSize();
                return new RectangleF(Position2.X - size.X, Position2.Y - size.Y, 2 * size.X, 2 * size.Y);
            }
        }

        public RectTransform(Vector2 offset, Vector2 pivot, SizeF size, Anchor anchor) : 
            base(Vector3.Zero, Vector3.Zero, Vector3.One)
        {
            Offset = offset;
            Pivot = pivot;
            Size = size;
            Anchor = anchor;
        }

        public RectTransform(Vector2 offset, Anchor anchor) : this(offset, Vector2.Zero, SizeF.Empty, anchor) { }

        public Matrix3x2 GetTransformMatrix()
        {
            var leftUp = Position2 - CalculateHalfSize();
            var rotation = Rotation.Z * (float)Math.PI / 180;
            return Matrix.Transformation2D(Pivot, 0, (Vector2)Scale, Pivot, rotation, leftUp);
        }

        public Vector2 CalculateHalfSize()
        {
            var size = new Vector2(Size.Width * Scale.X, Size.Height * Scale.Y) / 2;

            if (IsPivot)
            {
                Pivot = new Vector2(size.X, size.Y);
            }

            return size;
        }
    }
}
