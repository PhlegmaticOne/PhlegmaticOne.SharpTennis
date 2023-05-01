using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Transform;
using SharpDX;
using System.Drawing;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Pause
{
    public class PausePopupFactory : PopupFactory<PausePopup>
    {
        public PausePopupFactory(PausePopup popup) : base(popup) { }

        public override Canvas SetupPopup(PausePopup popup)
        {
            var textFormat = TextFormatData.DefaultForSize(50);
            var background = CreateBackground();
            var exitButton = CreateExitButton(textFormat);
            var continueButton = CreateContinueButton(textFormat);
            var settingsButton = CreateSettingsButton(textFormat);
            var restartButton = CreateRestartButton(textFormat);

            var canvas = Canvas.Create("PauseCanvas", background,
                exitButton.GameObject, continueButton.GameObject, settingsButton.GameObject, restartButton.GameObject,
                CreateInfoText(new Vector2(0, -360), "Pause"));
            popup.Setup(continueButton, exitButton, settingsButton, restartButton);
            return canvas;
        }

        private GameObject CreateInfoText(Vector2 offset, string message)
        {
            var font = 50;
            var go = new GameObject();
            var text = TextComponent.Create(Colors.White, message, TextFormatData.DefaultForSize(font));
            text.RectTransform.Anchor = Anchor.Center;
            text.RectTransform.Offset = offset;
            text.RectTransform.Size = new SizeF(600, font);
            go.AddComponent(new ResizableComponent(text.RectTransform), false);
            go.AddComponent(text, false);
            return go;
        }

        private ButtonComponent CreateContinueButton(TextFormatData textFormatData)
        {
            var go = new GameObject();
            var image = ImageComponent.Create(@"assets\textures\ui\accept_button.png",
                new Vector2(-320, -200), Anchor.Center);
            var text = TextComponent.Create(Colors.White, "Continue", textFormatData);
            var playButton = new ButtonComponent(image, text);
            go.AddComponent(image, false);
            go.AddComponent(text, false);
            go.AddComponent(playButton, false);
            return playButton;
        }

        private ButtonComponent CreateRestartButton(TextFormatData textFormatData)
        {
            var go = new GameObject();
            var image = ImageComponent.Create(@"assets\textures\ui\restart_button.png",
                new Vector2(320, -200), Anchor.Center);
            var text = TextComponent.Create(Colors.White, "Restart", textFormatData);
            var playButton = new ButtonComponent(image, text);
            go.AddComponent(image, false);
            go.AddComponent(text, false);
            go.AddComponent(playButton, false);
            return playButton;
        }

        private ButtonComponent CreateSettingsButton(TextFormatData textFormatData)
        {
            var go = new GameObject();
            var image = ImageComponent.Create(@"assets\textures\ui\settings_button.png",
                new Vector2(0, 30), Anchor.Center);
            var text = TextComponent.Create(Colors.White, "Settings", textFormatData);
            var playButton = new ButtonComponent(image, text);
            go.AddComponent(image, false);
            go.AddComponent(text, false);
            go.AddComponent(playButton, false);
            return playButton;
        }

        private ButtonComponent CreateExitButton(TextFormatData textFormatData)
        {
            var go = new GameObject();
            var image = ImageComponent.Create(@"assets\textures\ui\cancel_button.png",
                new Vector2(0, 260), Anchor.Center);
            var text = TextComponent.Create(Colors.White, "Exit", textFormatData);
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
    }
}
