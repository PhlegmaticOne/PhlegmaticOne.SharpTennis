using System.Drawing;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Transform;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using static System.Net.Mime.MediaTypeNames;
using FontStyle = SharpDX.DirectWrite.FontStyle;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface
{
    public class GameCanvasFactory : ICanvasFactory
    {
        public Canvas CreateCanvas()
        {
            var textFormatData = CreateGameTextFormatData();

            var canvas = Canvas.Create("GameCanvas",
                CreateScoreText(textFormatData, Colors.White, Anchor.TopLeft, "You", true),
                CreateScoreText(textFormatData, Colors.White, Anchor.TopRight, "Enemy", false));

            return canvas;
        }

        private GameObject CreateBallFlyView(TextFormatData textFormatData, RawColor4 color, Anchor anchor)
        {
            var go = new GameObject();
            var ballViewText = TextComponent.Create(color, string.Empty, textFormatData);
            ballViewText.RectTransform.Anchor = anchor;
            ballViewText.RectTransform.Size = new SizeF(500, 200);
            go.AddComponent(ballViewText, false);
            var playerScore = new BallFlyView(ballViewText);
            go.AddComponent(playerScore, false);
            go.AddComponent(new ResizableComponent(ballViewText.RectTransform), false);
            return go;
        }

        private GameObject CreateScoreText(TextFormatData textFormatData, RawColor4 color, Anchor anchor, string text, bool isPlayer)
        {
            var go = new GameObject();
            var playerScoreText = TextComponent.Create(color, string.Empty, textFormatData);
            playerScoreText.RectTransform.Anchor = anchor;
            go.AddComponent(playerScoreText, false);
            var playerScore = new ScoreText(playerScoreText, text, isPlayer);
            go.AddComponent(playerScore, false);
            go.AddComponent(new ResizableComponent(playerScoreText.RectTransform), false);
            return go;
        }

        private TextFormatData CreateGameTextFormatData()
        {
            return new TextFormatData
            {
                FontFamily = "Console",
                FontSize = 50,
                FontStretch = FontStretch.Normal,
                FontStyle = FontStyle.Normal,
                FontWeight = FontWeight.Bold,
                ParagraphAlignment = ParagraphAlignment.Center,
                TextAlignment = TextAlignment.Center
            };
        }
    }
}
