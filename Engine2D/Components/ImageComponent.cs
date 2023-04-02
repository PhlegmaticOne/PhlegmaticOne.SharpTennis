using System;
using System.Drawing;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Transform;
using SharpDX;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using Direct2DBitmapInterpolationMode = SharpDX.Direct2D1.BitmapInterpolationMode;

namespace PhlegmaticOne.SharpTennis.Game.Engine2D.Components
{
    public class ImageComponent : RectComponent, IResizable
    {
        public static ImageComponent Create(string fileName, Vector2 offset, Anchor anchor)
        {
            return new ImageComponent
            {
                FileName = fileName,
                Opacity = 1f,
                InterpolationMode = Direct2DBitmapInterpolationMode.Linear,
                RectTransform = new RectTransform(offset, anchor)
            };
        }

        public override void Dispose()
        {
            var bitmap = Bitmap;
            Utilities.Dispose(ref bitmap);
        }

        public Bitmap Bitmap { get; private set; }
        public string FileName { get; set; }
        public float Opacity { get; set; } = 1f;
        public Direct2DBitmapInterpolationMode InterpolationMode { get; set; } = Direct2DBitmapInterpolationMode.Linear;

        public void SetBitmap(Bitmap bitmap)
        {
            Bitmap = bitmap;
            RectTransform.Size = new SizeF(bitmap.Size.Width, bitmap.Size.Height);
        }
    }
}
