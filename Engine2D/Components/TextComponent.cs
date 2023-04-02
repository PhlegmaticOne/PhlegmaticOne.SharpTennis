using PhlegmaticOne.SharpTennis.Game.Engine2D.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Transform;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;

namespace PhlegmaticOne.SharpTennis.Game.Engine2D.Components
{
    public class TextComponent : RectComponent
    {
        public static TextComponent Create(RawColor4 color, string text, TextFormatData textFormatData)
        {
            return new TextComponent
            {
                BrushColor = color,
                RectTransform = RectTransform.RectIdentity,
                Text = text,
                TextFormatData = textFormatData
            };
        }

        public override void Dispose()
        {
            var brush = Brush;
            var textFormat = TextFormat;

            Utilities.Dispose(ref brush);
            Utilities.Dispose(ref textFormat);
        }

        public TextFormat TextFormat { get; set; }
        public SolidColorBrush Brush { get; set; }
        public TextFormatData TextFormatData { get; set; }
        public RawColor4 BrushColor { get; set; }
        public string Text { get; set; }
    }
}
