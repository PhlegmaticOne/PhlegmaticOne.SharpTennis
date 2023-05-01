using PhlegmaticOne.SharpTennis.Game.Common.Commands;
using PhlegmaticOne.SharpTennis.Game.Common.Sound.Models;
using PhlegmaticOne.SharpTennis.Game.Common.Sound.Models.Data;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;

namespace PhlegmaticOne.SharpTennis.Game.Game.Commands
{
    public class SaveSoundSettingsCommand : ICommand
    {
        private readonly ISoundSettingsProvider _soundSettingsProvider;
        private readonly PopupSystem _popupSystem;

        public SaveSoundSettingsCommand(ISoundSettingsProvider soundSettingsProvider, PopupSystem popupSystem)
        {
            _soundSettingsProvider = soundSettingsProvider;
            _popupSystem = popupSystem;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (!(parameter is SoundSettings soundSettings))
            {
                return;
            }

            _soundSettingsProvider.Settings.IsMuted = soundSettings.IsMuted;
            _soundSettingsProvider.Settings.Volume = soundSettings.Volume;
            _soundSettingsProvider.ForceSave();
            _popupSystem.CloseLastPopup();
        }
    }
}
