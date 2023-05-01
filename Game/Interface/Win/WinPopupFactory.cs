using System.Drawing;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Transform;
using SharpDX;
using SharpDX.DirectWrite;
using FontStyle = SharpDX.DirectWrite.FontStyle;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Win
{
    public class WinPopupFactory : PopupFactory<WinPopup>
    {
        public WinPopupFactory(WinPopup popup) : base(popup) { }

        public override Canvas SetupPopup(WinPopup popup)
        {
            var textFormat = CreateWinTextFormatData();
            var backGround = CreateBackground();
            var playButton = CreateContinueButton(textFormat);
            var exitButton = CreateExitButton(textFormat);
            var text = CreateWinText(textFormat);
            popup.Setup(playButton, exitButton, text);
            var canvas = Canvas.Create("WinPopup", backGround, playButton.GameObject, exitButton.GameObject, text.GameObject);
            return canvas;
        }

        private TextComponent CreateWinText(TextFormatData textFormatData)
        {
            var go = new GameObject();
            var text = TextComponent.Create(Colors.Yellow, "", textFormatData);
            text.RectTransform.Anchor = Anchor.Center;
            text.RectTransform.Offset = new Vector2(0, -250);
            text.RectTransform.Size = new SizeF(400, textFormatData.FontSize);
            go.AddComponent(new ResizableComponent(text.RectTransform), false);
            go.AddComponent(text, false);
            return text;
        }

        private ButtonComponent CreateExitButton(TextFormatData textFormatData)
        {
            var go = new GameObject();
            var image = ImageComponent.Create(@"assets\textures\ui\cancel_button.png",
                new Vector2(320, 250), Anchor.Center);
            var text = TextComponent.Create(Colors.White, "Exit", textFormatData);
            var playButton = new ButtonComponent(image, text);
            go.AddComponent(image, false);
            go.AddComponent(text, false);
            go.AddComponent(playButton, false);
            return playButton;
        }

        private ButtonComponent CreateContinueButton(TextFormatData textFormatData)
        {
            var go = new GameObject();
            var image = ImageComponent.Create(@"assets\textures\ui\accept_button.png", 
                new Vector2(-320, 250), Anchor.Center);
            var text = TextComponent.Create(Colors.White, "Continue", textFormatData);
            var playButton = new ButtonComponent(image, text);
            go.AddComponent(image, false);
            go.AddComponent(text, false);
            go.AddComponent(playButton, false);
            return playButton;
        }

        private GameObject CreateBackground()
        {
            var backGround = new GameObject("Background");
            var image = ImageComponent.Create(@"assets\textures\ui\win_popup.png", Vector2.Zero, Anchor.Center);
            backGround.AddComponent(image, false);
            return backGround;
        }

        private TextFormatData CreateWinTextFormatData()
        {
            return new TextFormatData
            {
                FontFamily = "Console",
                FontSize = 70,
                FontStretch = FontStretch.Normal,
                FontStyle = FontStyle.Normal,
                FontWeight = FontWeight.Bold,
                ParagraphAlignment = ParagraphAlignment.Center,
                TextAlignment = TextAlignment.Center
            };
        }
    }
}
