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

        public MenuPopup(MenuPopupViewModel menuCanvasViewModel,
            ISoundManager<GameSounds> soundManager) : base(soundManager)
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
