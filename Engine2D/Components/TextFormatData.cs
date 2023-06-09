﻿using SharpDX.DirectWrite;

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

        public static TextFormatData DefaultForSize(int fontSize) => new TextFormatData
        {
            FontFamily = "Console",
            FontSize = fontSize,
            FontStretch = FontStretch.Normal,
            FontStyle = FontStyle.Normal,
            FontWeight = FontWeight.Bold,
            ParagraphAlignment = ParagraphAlignment.Center,
            TextAlignment = TextAlignment.Center
        };

        public TextFormatData Clone()
        {
            return new TextFormatData
            {
                FontFamily = FontFamily,
                FontWeight = FontWeight,
                FontStyle = FontStyle,
                FontStretch = FontStretch,
                FontSize = FontSize,
                TextAlignment = TextAlignment,
                ParagraphAlignment = ParagraphAlignment
            };
        }
    }
}