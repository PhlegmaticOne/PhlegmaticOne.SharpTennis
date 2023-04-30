using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Menu
{
    public class MenuPopup : PopupBase
    {
        private readonly MenuCanvasViewModel _menuCanvasViewModel;

        private ButtonComponent _playButton;
        private ButtonComponent _exitButton;

        public MenuPopup(MenuCanvasViewModel menuCanvasViewModel)
        {
            _menuCanvasViewModel = menuCanvasViewModel;
        }

        public void Setup(ButtonComponent playButton, ButtonComponent exitButton)
        {
            _playButton = playButton;
            _exitButton = exitButton;
            _playButton.OnClick.Add(() => _menuCanvasViewModel.PlayButtonCommand.Execute(null));
            _exitButton.OnClick.Add(() => _menuCanvasViewModel.ExitButtonCommand.Execute(null));
        }

        public override void OnDestroy()
        {
            _playButton.OnClick.Clear();
            _exitButton.OnClick.Clear();
        }
    }
}
