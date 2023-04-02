using SharpDX.DirectWrite;

namespace PhlegmaticOne.SharpTennis.Game.Engine2D.Components
{
    public class TextFormatData
    {
        public string FontFamily { get; set; }
        public FontWeight FontWeight { get; set; }
        public FontStyle FontStyle { get; set; }
        public FontStretch FontStretch { get; set; }
        public float FontSize { get; set; }
        public TextAlignment TextAlignment { get; set; }
        public ParagraphAlignment ParagraphAlignment { get; set; }
    }
}