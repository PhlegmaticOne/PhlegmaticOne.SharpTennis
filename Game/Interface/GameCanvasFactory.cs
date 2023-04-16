using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Transform;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;

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
