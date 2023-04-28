using System.Collections.Generic;
using System.Drawing;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Base.Scenes;
using PhlegmaticOne.SharpTennis.Game.Common.Extensions;
using PhlegmaticOne.SharpTennis.Game.Engine2D;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Transform;
using PhlegmaticOne.SharpTennis.Game.Game.Controllers;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using FontStyle = SharpDX.DirectWrite.FontStyle;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Game
{
    public class GameCanvasFactory : ICanvasFactory
    {
        public Canvas CreateCanvasForScene(Scene scene)
        {
            var textFormatData = CreateGameTextFormatData();
            var scoreTexts = SetupScoreViews(scene, textFormatData);
            var gameStateView = SetupGameStateView(scene, textFormatData);

            var canvas = Canvas.Create("GameCanvas", scoreTexts
                .FluentAdd(gameStateView)
                .ToArray());

            return canvas;
        }

        private GameObject SetupGameStateView(Scene scene, TextFormatData textFormatData)
        {
            var data = textFormatData.Clone();
            data.FontSize = 40;
            var go = new GameObject();
            var text = TextComponent.Create(Colors.White, string.Empty, data);
            text.RectTransform.Anchor = Anchor.Top;
            text.RectTransform.Size = new SizeF(400, data.FontSize);
            var gameStateView = scene.GetComponent<GameStateViewController>();
            gameStateView.SetTextComponent(text);
            go.AddComponent(gameStateView, false);
            go.AddComponent(new ResizableComponent(text.RectTransform), false);
            go.AddComponent(text, false);
            return go;
        }

        private List<GameObject> SetupScoreViews(Scene scene, TextFormatData textFormatData)
        {
            var floorController = scene.GetComponent<BallFloorCollisionController>();
            var playerText = CreateScoreText(textFormatData, Colors.White, Anchor.TopLeft, "You", true);
            var enemyText = CreateScoreText(textFormatData, Colors.White, Anchor.TopRight, "Enemy", false);
            floorController.Setup(playerText, enemyText);
            return new List<GameObject> { playerText.GameObject, enemyText.GameObject };
        }


        private ScoreText CreateScoreText(TextFormatData textFormatData, RawColor4 color, Anchor anchor, string text, bool isPlayer)
        {
            var go = new GameObject();
            var playerScoreText = TextComponent.Create(color, string.Empty, textFormatData);
            playerScoreText.RectTransform.Anchor = anchor;
            go.AddComponent(playerScoreText, false);
            var playerScore = new ScoreText(playerScoreText, text, isPlayer);
            go.AddComponent(playerScore, false);
            go.AddComponent(new ResizableComponent(playerScoreText.RectTransform), false);
            return playerScore;
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
