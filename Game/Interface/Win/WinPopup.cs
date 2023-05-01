using PhlegmaticOne.SharpTennis.Game.Common.Sound.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Win
{
    public class WinPopup : GamePopupBase
    {
        private readonly WinPopupViewModel _winPopupViewModel;
        private ButtonComponent _continueButton;
        private ButtonComponent _exitButton;
        private TextComponent _winText;

        public WinPopup(WinPopupViewModel winPopupViewModel, ISoundManager<GameSounds> soundManager) : base(soundManager)
        {
            _winPopupViewModel = winPopupViewModel;
        }


        public void Setup(ButtonComponent continueButton, ButtonComponent exitButton, TextComponent winText)
        {
            _continueButton = continueButton;
            _exitButton = exitButton;
            _winText = winText;

            _continueButton.OnClick.Add(() => _winPopupViewModel.ContinueGameCommand.Execute(null));
            _exitButton.OnClick.Add(() => _winPopupViewModel.ExitGameCommand.Execute(null));
        }

        public void SetWinner(RacketType racket) => _winText.Text = "Winner: " + racket;
    }
}
