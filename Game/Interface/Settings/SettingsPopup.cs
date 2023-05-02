﻿using System.Linq;
using PhlegmaticOne.SharpTennis.Game.Common.Sound.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Sound.Models;
using PhlegmaticOne.SharpTennis.Game.Common.Sound.Models.Data;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game.Player.Data;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Settings
{
    public class SettingsPopup : GamePopupBase
    {
        private readonly SettingsPopupViewModel _settingsPopupViewModel;
        private readonly ISoundSettingsProvider _soundSettingsProvider;
        private readonly IPlayerDataProvider _playerDataProvider;

        private ButtonComponent _closeButton;
        private ButtonComponent _saveButton;
        private InputNumberSelectableElement _volumeElement;
        private CheckBox _isMutedCheckBox;
        private ButtonComponent _backButton;
        private InputStringSelectableElement _nameElement;

        public SettingsPopup(SettingsPopupViewModel settingsPopupViewModel,
            ISoundSettingsProvider soundSettingsProvider,
            IPlayerDataProvider playerDataProvider,
            ISoundManager<GameSounds> soundManager) : base(soundManager)
        {
            _settingsPopupViewModel = settingsPopupViewModel;
            _soundSettingsProvider = soundSettingsProvider;
            _playerDataProvider = playerDataProvider;
        }

        public void Setup(ButtonComponent backButton, ButtonComponent closeButton, ButtonComponent saveButton,
            InputNumberSelectableElement volumeElement, CheckBox isMutedCheckBox,
            InputStringSelectableElement nameElement)
        {
            _closeButton = closeButton;
            _saveButton = saveButton;
            _volumeElement = volumeElement;
            _isMutedCheckBox = isMutedCheckBox;
            _backButton = backButton;
            _nameElement = nameElement;

            _backButton.OnClick.Add(DeselectsComponents);
            _closeButton.OnClick.Add(() => _settingsPopupViewModel.CloseCommand.Execute(null));
            _saveButton.OnClick.Add(() =>
            {
                var newVolume = _volumeElement.NumberValue;
                var changeVolume = !(newVolume == -1 || newVolume > 100);
                var isMute = _isMutedCheckBox.IsSelected;
                _settingsPopupViewModel.SaveCommand.Execute(new SoundSettings
                {
                    IsMuted = isMute,
                    Volume = changeVolume ? newVolume : _soundSettingsProvider.Settings.Volume
                });

                var newName = _nameElement.Value;
                if (string.IsNullOrWhiteSpace(newName))
                {
                    return;
                }
                _playerDataProvider.PlayerData.Name = newName;
                _playerDataProvider.ForceSave();
            });
        }

        protected override void OnShow()
        {
            _isMutedCheckBox.Check(_soundSettingsProvider.Settings.IsMuted);
            base.OnShow();
        }

        protected override void OnClose()
        {
            _backButton.OnClick.Clear();
            _closeButton.OnClick.Clear();
            _saveButton.OnClick.Clear();
            base.OnClose();
        }

        private void DeselectsComponents()
        {
            foreach (var selectable in Canvas.GetElements().OfType<Selectable>().Where(x => x.DeselectOnMisClick))
            {
                selectable.Deselect();
            }
        }
    }
}
