using System.Collections.Generic;
using System.Drawing;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Extensions;
using PhlegmaticOne.SharpTennis.Game.Engine2D;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Transform;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Base;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.GameSettings
{
    public class GameSettingPopupFactory : PopupFactory<GameSettingsPopup>
    {
        private readonly InputNumberSelectableElement _inputNumberSelectableElement;

        public GameSettingPopupFactory(GameSettingsPopup popup, InputNumberSelectableElement inputNumberSelectableElement) :
            base(popup)
        {
            _inputNumberSelectableElement = inputNumberSelectableElement;
        }

        public override Canvas SetupPopup(GameSettingsPopup popup)
        {
            var background = CreateBackground();
            var difficultyPanel = CreateSelectDifficultyPanel();
            var colorsPanel = CreateSelectColorPanel();

            _inputNumberSelectableElement.Setup(@"assets\textures\ui\GameSettings\rounds.png", 80, "5");
            _inputNumberSelectableElement.RectTransform.Offset = new Vector2(0, 220);

            var closeButton = CreateCloseButton();
            var startButton = CreateStartGameButton();


            var canvas = Canvas.Create("GameSettings", new List<GameObject>()
                .FluentAdd(background)
                .FluentAddRange(difficultyPanel.SelectableObjects)
                .FluentAddRange(colorsPanel.SelectableObjects)
                .FluentAdd(_inputNumberSelectableElement.GameObject)
                .FluentAdd(closeButton.GameObject)
                .FluentAdd(startButton.GameObject)
                .FluentAdd(CreateInfoText(new Vector2(0, -450), "Setup your game"))
                .FluentAdd(CreateInfoText(new Vector2(0, -360), "Choose difficulty:"))
                .FluentAdd(CreateInfoText(new Vector2(0, -130), "Choose your color:"))
                .FluentAdd(CreateInfoText(new Vector2(0, 100), "Enter rounds to play:"))
                .ToArray());
            popup.Setup(colorsPanel, difficultyPanel, _inputNumberSelectableElement, closeButton, startButton);
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

        private ButtonComponent CreateStartGameButton()
        {
            var go = new GameObject();
            var image = ImageComponent.Create(@"assets\textures\ui\GameSettings\start_game.png",
                new Vector2(0, 400), Anchor.Center);
            var text = TextComponent.Create(Colors.White, "Start game", TextFormatData.DefaultForSize(40));
            var playButton = new ButtonComponent(image, text);
            go.AddComponent(image, false);
            go.AddComponent(text, false);
            go.AddComponent(playButton, false);
            return playButton;
        }

        private ButtonComponent CreateCloseButton()
        {
            var go = new GameObject("AAAAA");
            var image = ImageComponent.Create(@"assets\textures\ui\GameSettings\close.png",
                new Vector2(600, -420), Anchor.Center);
            var text = TextComponent.Create(Colors.White, string.Empty, TextFormatData.DefaultForSize(50));
            var playButton = new ButtonComponent(image, text);
            go.AddComponent(image, false);
            go.AddComponent(text, false);
            go.AddComponent(playButton, false);
            return playButton;
        }

        private SelectablePanel<ColorType> CreateSelectColorPanel()
        {
            var fontSize = 40;
            var panel = SelectablePanel<ColorType>.Create(new List<SelectableComponent<ColorType>>
            {
                CreateSelectable(@"assets\textures\ui\GameSettings\red.png", fontSize, ColorType.Red),
                CreateSelectable(@"assets\textures\ui\GameSettings\black.png", fontSize, ColorType.Black)
            });
            panel.SetItemsOffset(350, new Vector2(0, -20), 20);
            return panel;
        }

        private SelectablePanel<DifficultyType> CreateSelectDifficultyPanel()
        {
            var fontSize = 40;
            var panel = SelectablePanel<DifficultyType>.Create(new List<SelectableComponent<DifficultyType>>
            {
                CreateSelectable(@"assets\textures\ui\GameSettings\easy.png", fontSize, DifficultyType.Easy),
                CreateSelectable(@"assets\textures\ui\GameSettings\medium.png", fontSize, DifficultyType.Medium),
                CreateSelectable(@"assets\textures\ui\GameSettings\hard.png", fontSize, DifficultyType.Hard),
                CreateSelectable(@"assets\textures\ui\GameSettings\impossible.png", fontSize, DifficultyType.Impossible),
            });
            panel.SetItemsOffset(250, new Vector2(0, -250), 20);
            return panel;
        }

        private SelectableComponent<T> CreateSelectable<T>(string image, int fontSize, T value)
        {
            return SelectableComponent<T>.Create(image, fontSize, value);
        }

        private GameObject CreateBackground()
        {
            var backGround = new GameObject("Background");
            var image = ImageComponent.Create(@"assets\textures\ui\GameSettings\popup.png", Vector2.Zero, Anchor.Center);
            backGround.AddComponent(image, false);
            return backGround;
        }
    }
}
