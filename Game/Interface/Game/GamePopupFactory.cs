using System.Collections.Generic;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Transform;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using System.Drawing;
using PhlegmaticOne.SharpTennis.Game.Common.Extensions;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game.Player.Data;
using SharpDX;
using FontStyle = SharpDX.DirectWrite.FontStyle;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Game
{
    public class GamePopupFactory : PopupFactory<GamePopup>
    {
        private readonly IPlayerDataProvider _playerDataProvider;
        public GamePopupFactory(GamePopup popup, IPlayerDataProvider playerDataProvider) : base(popup)
        {
            _playerDataProvider = playerDataProvider;
        }

        public override Canvas SetupPopup(GamePopup popup)
        {
            var textFormatData = CreateGameTextFormatData();
            var scoreSystem = SetupScoreViews(textFormatData);
            var gameStateView = SetupGameStateView(textFormatData);
            var infoText = CreateMessageText(new Vector2(50, -50));

            var canvas = Canvas.Create("GameCanvas", new List<GameObject>()
                .FluentAdd(gameStateView.GameObject)
                .FluentAdd(scoreSystem.PlayerText.GameObject)
                .FluentAdd(scoreSystem.EnemyText.GameObject)
                .FluentAdd(infoText.GameObject)
                .ToArray());

            popup.SetupViews(scoreSystem, gameStateView, infoText);
            return canvas;
        }

        private TextComponent CreateMessageText(Vector2 offset)
        {
            var font = 40;
            var go = new GameObject();
            var text = TextComponent.Create(Colors.White, string.Empty, TextFormatData.DefaultForSize(font));
            text.TextFormatData.TextAlignment = TextAlignment.Leading;
            text.RectTransform.Anchor = Anchor.BottomLeft;
            text.RectTransform.Offset = offset;
            text.RectTransform.Size = new SizeF(400, font);
            go.AddComponent(new ResizableComponent(text.RectTransform), false);
            go.AddComponent(text, false);
            return text;
        }

        private GameStateViewController SetupGameStateView(TextFormatData textFormatData)
        {
            var data = textFormatData.Clone();
            data.FontSize = 40;
            var go = new GameObject();
            var text = TextComponent.Create(Colors.White, string.Empty, data);
            text.RectTransform.Anchor = Anchor.Top;
            text.RectTransform.Size = new SizeF(1000, data.FontSize);
            var gameStateSwitcher = new GameStateViewController(_playerDataProvider);
            gameStateSwitcher.SetTextComponent(text);
            go.AddComponent(gameStateSwitcher, false);
            go.AddComponent(new ResizableComponent(text.RectTransform), false);
            go.AddComponent(text, false);
            return gameStateSwitcher;
        }

        private ScoreSystem SetupScoreViews(TextFormatData textFormatData)
        {
            var scoreSystem = new ScoreSystem(_playerDataProvider);
            var playerText = CreateScoreText(textFormatData, Colors.White, Anchor.TopLeft, 
                _playerDataProvider.PlayerData.Name, true);
            var enemyText = CreateScoreText(textFormatData, Colors.White, Anchor.TopRight, "Enemy", false);
            scoreSystem.PlayerText = playerText;
            scoreSystem.EnemyText = enemyText;
            return scoreSystem;
        }


        private ScoreText CreateScoreText(TextFormatData textFormatData, RawColor4 color, Anchor anchor, string text, bool isPlayer)
        {
            var go = new GameObject();
            var playerScoreText = TextComponent.Create(color, string.Empty, textFormatData);
            playerScoreText.RectTransform.Anchor = anchor;
            var playerScore = new ScoreText(playerScoreText, text, isPlayer);
            go.AddComponent(playerScore, false);
            go.AddComponent(playerScoreText, false);
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
