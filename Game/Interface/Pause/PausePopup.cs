using PhlegmaticOne.SharpTennis.Game.Common.Sound.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Pause
{
    public class PausePopup : GamePopupBase
    {
        private readonly PausePopupViewModel _pausePopupViewModel;
        private ButtonComponent _continueButton;
        private ButtonComponent _exitButton;
        private ButtonComponent _settingsButton;
        private ButtonComponent _restartButton;

        private bool _isContinue = true;
        private bool _isContinueButtonPressed;
        private bool _restartButtonPressed;

        public PausePopup(PausePopupViewModel pausePopupViewModel, ISoundManager<GameSounds> soundManager) :
            base(soundManager)
        {
            _pausePopupViewModel = pausePopupViewModel;
        }

        public void Setup(ButtonComponent continueButton, ButtonComponent exitButton, 
            ButtonComponent settingsButton, ButtonComponent restartButton)
        {
            _continueButton = continueButton;
            _exitButton = exitButton;
            _settingsButton = settingsButton;
            _restartButton = restartButton;

            _continueButton.OnClick.Add(() =>
            {
                _isContinueButtonPressed = true;
                _pausePopupViewModel.OnContinueCommand.Execute(null);
            });

            _exitButton.OnClick.Add(() =>
            {
                _isContinue = false;
                _pausePopupViewModel.OnExitCommand.Execute(null);
            });

            _settingsButton.OnClick.Add(() => _pausePopupViewModel.OnSettingsCommand.Execute(null));

            _restartButton.OnClick.Add(() =>
            {
                _restartButtonPressed = true;
                _pausePopupViewModel.RestartCommand.Execute(null);
            });
        }

        protected override void OnShow()
        {
            _pausePopupViewModel.OnShowCommand.Execute(null);
            base.OnShow();
        }

        protected override void OnClose()
        {
            if (_isContinue && _isContinueButtonPressed == false && _restartButtonPressed == false)
            {
                _pausePopupViewModel.OnContinueCommand.Execute(false);
            }

            _continueButton.OnClick.Clear();
            _exitButton.OnClick.Clear();
            _settingsButton.OnClick.Clear();
            _restartButton.OnClick.Clear();
            _isContinue = true;
            _isContinueButtonPressed = false;
            _restartButtonPressed = false;
            base.OnClose();
        }
    }
}
