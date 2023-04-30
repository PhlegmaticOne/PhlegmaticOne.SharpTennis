using System;
using PhlegmaticOne.SharpTennis.Game.Common.Infrastructure;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using SharpDX.WIC;
using Direct2DFactory = SharpDX.Direct2D1.Factory;
using WriteFactory = SharpDX.DirectWrite.Factory;
using Direct2DBitmap = SharpDX.Direct2D1.Bitmap;
using Direct2DPixelFormat = SharpDX.Direct2D1.PixelFormat;
using Direct2DAlphaMode = SharpDX.Direct2D1.AlphaMode;
using Direct2DTextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode;
using WicPixelFormat = SharpDX.WIC.PixelFormat;
using Direct2DBitmapInterpolationMode = SharpDX.Direct2D1.BitmapInterpolationMode;

namespace PhlegmaticOne.SharpTennis.Game.Engine2D.DirectX
{
    public class DirectX2DGraphics : IDisposable, IElementsCreator, IElementsRenderer
    {
        private readonly RenderTargetProperties _renderTargetProperties;
        private Direct2DFactory _factory;
        private WriteFactory _writeFactory;
        private ImagingFactory _imagingFactory;
        private RenderTarget _renderTarget;

        public DirectX2DGraphics()
        {
            _factory = new Direct2DFactory();
            _writeFactory = new WriteFactory();
            _imagingFactory = new ImagingFactory();
            _renderTargetProperties.DpiX = 0;
            _renderTargetProperties.DpiY = 0;
            _renderTargetProperties.MinLevel = FeatureLevel.Level_10;
            _renderTargetProperties.PixelFormat = new Direct2DPixelFormat(Format.Unknown, Direct2DAlphaMode.Premultiplied);
            _renderTargetProperties.Type = RenderTargetType.Hardware;
            _renderTargetProperties.Usage = RenderTargetUsage.None;
        }

        public void Resize(Surface surface)
        {
            _renderTarget = new RenderTarget(_factory, surface, _renderTargetProperties);
            Utilities.Dispose(ref surface);
            _renderTarget.AntialiasMode = AntialiasMode.PerPrimitive;
            _renderTarget.TextAntialiasMode = Direct2DTextAntialiasMode.Cleartype;
        }

        public Direct2DBitmap CreateImage(string fileName)
        {
            var decoder = new BitmapDecoder(_imagingFactory, fileName, DecodeOptions.CacheOnDemand);
            var bitmapFirstFrame = decoder.GetFrame(0);
            var imageFormatConverter = new FormatConverter(_imagingFactory);
            imageFormatConverter.Initialize(bitmapFirstFrame,
                WicPixelFormat.Format32bppPRGBA,
                BitmapDitherType.Ordered4x4, null, 0.0, BitmapPaletteType.Custom);
            var bitmap = Direct2DBitmap.FromWicBitmap(_renderTarget, imageFormatConverter);

            Utilities.Dispose(ref imageFormatConverter);
            Utilities.Dispose(ref decoder);

            return bitmap;
        }

        public SolidColorBrush CreateBrush(RawColor4 color) => new SolidColorBrush(_renderTarget, color);

        public TextFormat CreateTextFormat(TextFormatData t)
        {
            var textFormat = new TextFormat(_writeFactory, t.FontFamily, t.FontWeight, t.FontStyle, t.FontStretch, t.FontSize);
            textFormat.TextAlignment = t.TextAlignment;
            textFormat.ParagraphAlignment = t.ParagraphAlignment;
            return textFormat;
        }

        public void BeginRender() => _renderTarget.BeginDraw();

        public void EndRender() => _renderTarget.EndDraw();

        public void RenderText(TextComponent textComponent)
        {
            if (GlobalVariables.IsDisposed)
            {
                return;
            }

            if (textComponent.Brush == null || textComponent.Brush.IsDisposed)
            {
                return;
            }

            _renderTarget.Transform = textComponent.RectTransform.GetTransformMatrix();
            _renderTarget.DrawText(textComponent.Text, textComponent.TextFormat, 
                textComponent.RectTransform.RawBounds, textComponent.Brush);
        }

        public void RenderImage(ImageComponent imageComponent)
        {
            if (GlobalVariables.IsDisposed)
            {
                return;
            }

            if (imageComponent.Bitmap == null || imageComponent.Bitmap.IsDisposed)
            {
                return;
            }

            _renderTarget.Transform = imageComponent.RectTransform.GetTransformMatrix();
            _renderTarget.DrawBitmap(imageComponent.Bitmap, imageComponent.Opacity, imageComponent.InterpolationMode);
        }

        public void DisposeOnResizing() => Utilities.Dispose(ref _renderTarget);

        public void Dispose()
        {
            Utilities.Dispose(ref _renderTarget);
            Utilities.Dispose(ref _imagingFactory);
            Utilities.Dispose(ref _writeFactory);
            Utilities.Dispose(ref _factory);
        }
    }
}
