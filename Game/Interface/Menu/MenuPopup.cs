using PhlegmaticOne.SharpTennis.Game.Common.Sound.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Menu
{
    public class MenuPopup : GamePopupBase
    {
        private readonly MenuPopupViewModel _menuCanvasViewModel;

        private ButtonComponent _playButton;
        private ButtonComponent _exitButton;
        private ButtonComponent _settingsButton;

        public MenuPopup(MenuPopupViewModel menuCanvasViewModel,
            ISoundManager<GameSounds> soundManager) : base(soundManager)
        {
            _menuCanvasViewModel = menuCanvasViewModel;
        }

        public void Setup(ButtonComponent playButton, ButtonComponent exitButton, ButtonComponent settingsButton)
        {
            _playButton = playButton;
            _exitButton = exitButton;
            _settingsButton = settingsButton;
            _playButton.OnClick.Add(() => _menuCanvasViewModel.PlayButtonCommand.Execute(null));
            _exitButton.OnClick.Add(() => _menuCanvasViewModel.ExitButtonCommand.Execute(null));
            _settingsButton.OnClick.Add(() => _menuCanvasViewModel.SettingsClickCommand.Execute(null));
        }

        public override void OnDestroy()
        {
            _playButton.OnClick.Clear();
            _exitButton.OnClick.Clear();
            _settingsButton.OnClick.Clear();
        }
    }
}
