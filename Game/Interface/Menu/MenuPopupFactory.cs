﻿using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Transform;
using SharpDX.DirectWrite;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Menu
{
    public class MenuPopupFactory : PopupFactory<MenuPopup>
    {
        public MenuPopupFactory(MenuPopup popup) : base(popup) { }

        public override Canvas SetupPopup(MenuPopup popup)
        {
            const string name = "MenuCanvas";
            var menuTextFormat = CreateMenuTextFormatData();

            var background = CreateBackground();
            var exitButton = CreateExitButton(menuTextFormat);
            var playButton = CreatePlayButton(menuTextFormat);
            var settingsButton = CreateSettingsButton();

            var canvas = Canvas.Create(name, background, playButton.GameObject, exitButton.GameObject, settingsButton.GameObject);
            popup.Setup(playButton, exitButton, settingsButton);
            return canvas;
        }

        private ButtonComponent CreateSettingsButton()
        {
            var playButtonObject = new GameObject();
            var playButtonImage = ImageComponent.Create(@"assets\textures\ui\Settings\icon.png",
                new Vector2(-60, 40), Anchor.TopRight);
            var playButtonText = TextComponent.Create(Colors.White, string.Empty, TextFormatData.DefaultForSize(2));
            var playButton = new ButtonComponent(playButtonImage, playButtonText);

            playButtonObject.AddComponent(playButtonImage, false);
            playButtonObject.AddComponent(playButtonText, false);
            playButtonObject.AddComponent(playButton, false);
            return playButton;
        }

        private ButtonComponent CreatePlayButton(TextFormatData textFormatData)
        {
            var playButtonObject = new GameObject();
            var playButtonImage = ImageComponent.Create(@"assets\textures\ui\play.png",
                Vector2.Zero, Anchor.Center);
            var playButtonText = TextComponent.Create(Colors.White, "Play", textFormatData);
            var playButton = new ButtonComponent(playButtonImage, playButtonText);

            playButtonObject.AddComponent(playButtonImage, false);
            playButtonObject.AddComponent(playButtonText, false);
            playButtonObject.AddComponent(playButton, false);
            return playButton;
        }

        private ButtonComponent CreateExitButton(TextFormatData textFormatData)
        {
            var exitButtonObject = new GameObject();
            var exitButtonImage = ImageComponent.Create(@"assets\textures\ui\exit.png",
                new Vector2(0, 150), Anchor.Center);
            var exitButtonText = TextComponent.Create(Colors.White, "Exit", textFormatData);
            var exitButton = new ButtonComponent(exitButtonImage, exitButtonText);

            exitButtonObject.AddComponent(exitButtonImage, false);
            exitButtonObject.AddComponent(exitButtonText, false);
            exitButtonObject.AddComponent(exitButton, false);
            return exitButton;
        }

        private GameObject CreateBackground()
        {
            var background = new GameObject();
            var image = ImageComponent
                .Create(@"assets\textures\ui\menu_back.jpg", Vector2.Zero, Anchor.Stretch);
            background.AddComponent(image, false);
            return background;
        }

        private TextFormatData CreateMenuTextFormatData()
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
