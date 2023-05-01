using System.Collections.Generic;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Transform;
using SharpDX;
using System.Drawing;
using System.Globalization;
using PhlegmaticOne.SharpTennis.Game.Common.Extensions;
using PhlegmaticOne.SharpTennis.Game.Common.Sound.Models.Data;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Settings
{
    public class SettingsPopupFactory : PopupFactory<SettingsPopup>
    {
        private readonly InputNumberSelectableElement _inputNumberSelectableElement;
        private readonly ISoundSettingsProvider _soundSettingsProvider;

        public SettingsPopupFactory(SettingsPopup popup,
            InputNumberSelectableElement inputNumberSelectableElement,
            ISoundSettingsProvider soundSettingsProvider) :
            base(popup)
        {
            _inputNumberSelectableElement = inputNumberSelectableElement;
            _soundSettingsProvider = soundSettingsProvider;
        }

        public override Canvas SetupPopup(SettingsPopup popup)
        {
            var background = CreateBackground();
            var closeButton = CreateCloseButton();
            var checkBox = CreateMuteSoundCheckBox();
            var saveButton = CreateSaveButton(TextFormatData.DefaultForSize(50));
            _inputNumberSelectableElement.DeselectOnMisClick = true;
            _inputNumberSelectableElement.Setup(@"assets\textures\ui\GameSettings\rounds.png", 80, 
                _soundSettingsProvider.Settings.Volume.ToString(CultureInfo.InvariantCulture));
            _inputNumberSelectableElement.RectTransform.Offset = new Vector2(0, -70);

            var canvas = Canvas.Create("SettingsCanvas", new List<GameObject>()
                .FluentAdd(background.GameObject)
                .FluentAdd(closeButton.GameObject)
                .FluentAdd(checkBox.GameObject)
                .FluentAddRange(checkBox.ImageObjects)
                .FluentAdd(_inputNumberSelectableElement.GameObject)
                .FluentAdd(saveButton.GameObject)
                .FluentAdd(CreateInfoText(new Vector2(0, -320), "Settings"))
                .FluentAdd(CreateInfoText(new Vector2(0, -200), "Sound volume:"))
                .FluentAdd(CreateInfoText(new Vector2(-100, 100), "Mute sound:"))
                .ToArray());
            popup.Setup(background, closeButton, saveButton, _inputNumberSelectableElement, checkBox);
            return canvas;
        }

        private ButtonComponent CreateSaveButton(TextFormatData textFormatData)
        {
            var go = new GameObject();
            var image = ImageComponent.Create(@"assets\textures\ui\GameSettings\start_game.png",
                new Vector2(0, 270), Anchor.Center);
            var text = TextComponent.Create(Colors.White, "Save", textFormatData);
            var playButton = new ButtonComponent(image, text);
            go.AddComponent(image, false);
            go.AddComponent(text, false);
            go.AddComponent(playButton, false);
            return playButton;
        }

        private CheckBox CreateMuteSoundCheckBox()
        {
            var checkBox = CheckBox.Create(
                @"assets\textures\ui\Settings\check_back.png",
                @"assets\textures\ui\Settings\check_mark.png");
            checkBox.SetOffset(new Vector2(150, 100));
            return checkBox;
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

        private ButtonComponent CreateCloseButton()
        {
            var go = new GameObject();
            var image = ImageComponent.Create(@"assets\textures\ui\GameSettings\close.png",
                new Vector2(590, -300), Anchor.Center);
            var text = TextComponent.Create(Colors.White, string.Empty, TextFormatData.DefaultForSize(50));
            var playButton = new ButtonComponent(image, text);
            go.AddComponent(image, false);
            go.AddComponent(text, false);
            go.AddComponent(playButton, false);
            return playButton;
        }

        private ButtonComponent CreateBackground()
        {
            var backGround = new GameObject("Background");
            var image = ImageComponent.Create(@"assets\textures\ui\win_popup.png", Vector2.Zero, Anchor.Center);
            var button = new ButtonComponent(image);
            backGround.AddComponent(image, false);
            backGround.AddComponent(button, false);
            return button;
        }
    }
}
