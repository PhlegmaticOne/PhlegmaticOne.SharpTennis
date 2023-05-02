using PhlegmaticOne.SharpTennis.Game.Common.Commands;
using PhlegmaticOne.SharpTennis.Game.Common.Sound.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game;
using System.Linq;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.GameSettings
{
    public class GameSettingsPopup : GamePopupBase
    {
        private readonly GameDataProvider _gameDataProvider;
        private readonly GameSettingsViewModel _viewModel;

        private SelectablePanel<ColorType> _colorPanel;
        private SelectablePanel<DifficultyType> _difficultyPanel;
        private InputNumberSelectableElement _scoreComponent;
        private InputNumberSelectableElement _timeComponent;
        private ButtonComponent _closeButton;
        private ButtonComponent _submitButton;
        private TextComponent _header;
        private CheckBox _scoreCheckBox;
        private CheckBox _timeCheckBox;
        private ButtonComponent _backButton;


        public GameSettingsPopup(GameDataProvider gameDataProvider, GameSettingsViewModel viewModel,
            ISoundManager<GameSounds> soundManager) : base(soundManager)
        {
            _gameDataProvider = gameDataProvider;
            _viewModel = viewModel;
        }

        public void SetupHeaderText(string text) => _header.Text = text;

        public void ResetStartGameCommand()
        {
            SetupStartGameCommand(_viewModel.StartGameCommand, null);
        }

        public void SetupStartGameCommand(ICommand startGameCommand, object value)
        {
            _submitButton.OnClick.Clear();
            _submitButton.OnClick.Add(() =>
            {
                SetupGameData();
                startGameCommand.Execute(value);
            });
        }

        public void Setup(SelectablePanel<ColorType> colorsPanel, 
            SelectablePanel<DifficultyType> difficultyPanel,
            InputNumberSelectableElement scoreComponent,
            InputNumberSelectableElement timeComponent,
            CheckBox scoreCheckBox,
            CheckBox timeCheckBox,
            ButtonComponent closeComponent,
            ButtonComponent submitButton,
            TextComponent header,
            ButtonComponent backButton)
        {
            _timeComponent = timeComponent;
            _scoreCheckBox = scoreCheckBox;
            _timeCheckBox = timeCheckBox;
            _colorPanel = colorsPanel;
            _difficultyPanel = difficultyPanel;
            _scoreComponent = scoreComponent;
            _closeButton = closeComponent;
            _submitButton = submitButton;
            _header = header;
            _backButton = backButton;

            _scoreCheckBox.Checked += ScoreCheckBoxOnChecked;
            _timeCheckBox.Checked += TimeCheckBoxOnChecked;
            _backButton.OnClick.Add(DeselectItems);
            _closeButton.OnClick.Add(() => _viewModel.CloseCommand.Execute(null));
        }

        private void TimeCheckBoxOnChecked(bool obj)
        {
            if (obj)
            {
                _scoreCheckBox.Check(false);
            }
            else
            {
                _timeCheckBox.Check(true);
            }
        }

        private void ScoreCheckBoxOnChecked(bool obj)
        {
            if (obj)
            {
                _timeCheckBox.Check(false);
            }
            else
            {
                _scoreCheckBox.Check(true);
            }
        }

        protected override void OnShow()
        {
            _timeCheckBox.Check(false);
            _scoreCheckBox.Check(true);
            ResetStartGameCommand();
        }

        private void SetupGameData()
        {
            var score = _scoreComponent.NumberValue;
            var timeInMinutes = _timeComponent.NumberValue;

            if (timeInMinutes == -1)
            {
                timeInMinutes = 1;
            }

            if (score == -1)
            {
                score = 11;
            }

            _gameDataProvider.SetGameData(new GameData
            {
                DifficultyType = _difficultyPanel.Value,
                PlayToScore = score,
                PlayerColor = _colorPanel.Value,
                TimeInMinutes = timeInMinutes,
                GameType = _scoreCheckBox.IsSelected ? GameType.Score : GameType.Time
            });
        }

        private void DeselectItems()
        {
            foreach (var selectable in Canvas.GetElements().OfType<Selectable>().Where(x => x.DeselectOnMisClick))
            {
                selectable.Deselect();
            }
        }

        protected override void OnClose()
        {
            _closeButton.OnClick.Clear();
            _submitButton.OnClick.Clear();
            _backButton.OnClick.Clear();
            _scoreCheckBox.Checked -= ScoreCheckBoxOnChecked;
            _timeCheckBox.Checked -= TimeCheckBoxOnChecked;
            base.OnClose();
        }
    }
}
