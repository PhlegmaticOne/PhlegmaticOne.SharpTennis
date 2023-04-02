using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Transform;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Base;
using SharpDX.DirectWrite;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface
{
    public class MenuCanvasFactory : ICanvasFactory
    {
        private readonly MenuCanvasViewModel _menuCanvasViewModel;

        public MenuCanvasFactory(MenuCanvasViewModel menuCanvasViewModel)
        {
            _menuCanvasViewModel = menuCanvasViewModel;
        }

        public Canvas CreateCanvas()
        {
            const string name = "MenuCanvas";
            var menuTextFormat = CreateMenuTextFormatData();

            var background = CreateBackground();
            var exitButton = CreateExitButton(menuTextFormat);
            var playButton = CreatePlayButton(menuTextFormat);

            var canvas = Canvas.Create(name, background, playButton, exitButton);
            return canvas;
        }

        private GameObject CreatePlayButton(TextFormatData textFormatData)
        {
            var playButtonObject = new GameObject();
            var playButtonImage = ImageComponent.Create(@"assets\textures\ui\play.png", 
                Vector2.Zero, Anchor.Center);
            var playButtonText = TextComponent.Create(Colors.White, "Play", textFormatData);
            var playButton = new ButtonComponent(playButtonImage, playButtonText);
            playButton.OnClick.Add(() => _menuCanvasViewModel.PlayButtonCommand.Execute(null));

            playButtonObject.AddComponent(playButtonImage, false);
            playButtonObject.AddComponent(playButtonText, false);
            playButtonObject.AddComponent(playButton, false);
            return playButtonObject;
        }

        private GameObject CreateExitButton(TextFormatData textFormatData)
        {
            var exitButtonObject = new GameObject();
            var exitButtonImage = ImageComponent.Create(@"assets\textures\ui\exit.png",
                new Vector2(0, 150), Anchor.Center);
            var exitButtonText = TextComponent.Create(Colors.White, "Exit", textFormatData);
            var exitButton = new ButtonComponent(exitButtonImage, exitButtonText);
            exitButton.OnClick.Add(() => _menuCanvasViewModel.ExitButtonCommand.Execute(null));

            exitButtonObject.AddComponent(exitButtonImage, false);
            exitButtonObject.AddComponent(exitButtonText, false);
            exitButtonObject.AddComponent(exitButton, false);
            return exitButtonObject;
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
