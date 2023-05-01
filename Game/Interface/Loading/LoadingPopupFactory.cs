using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Transform;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Loading
{
    public class LoadingPopupFactory : PopupFactory<LoadingPopup>
    {
        public LoadingPopupFactory(LoadingPopup popup) : base(popup) { }

        public override Canvas SetupPopup(LoadingPopup popup)
        {
            var back = CreateBackground();
            var loadingIcon = CreateLoadingIcon();
            var canvas = Canvas.Create("LoadingCanvas", back, loadingIcon.GameObject);
            popup.Setup(loadingIcon);
            return canvas;
        }

        private ImageComponent CreateLoadingIcon()
        {
            var background = new GameObject();
            var image = ImageComponent
                .Create(@"assets\textures\ui\Loading\loading_icon.png", Vector2.Zero, Anchor.Center);
            background.AddComponent(image, false);
            return image;
        }

        private GameObject CreateBackground()
        {
            var background = new GameObject();
            var image = ImageComponent
                .Create(@"assets\textures\ui\Loading\loading_back.jpg", Vector2.Zero, Anchor.Stretch);
            background.AddComponent(image, false);
            return background;
        }
    }
}
