using PhlegmaticOne.SharpTennis.Game.Common.Commands;
using PhlegmaticOne.SharpTennis.Game.Common.Sound.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.GameSettings
{
    public class GameSettingsPopup : GamePopupBase
    {
        private readonly GameDataProvider _gameDataProvider;
        private readonly GameSettingsViewModel _viewModel;

        private SelectablePanel<ColorType> _colorPanel;
        private SelectablePanel<DifficultyType> _difficultyPanel;
        private InputNumberSelectableElement _scoreComponent;
        private ButtonComponent _closeButton;
        private ButtonComponent _submitButton;
        private TextComponent _header;


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
            ButtonComponent closeComponent,
            ButtonComponent submitButton,
            TextComponent header)
        {
            _colorPanel = colorsPanel;
            _difficultyPanel = difficultyPanel;
            _scoreComponent = scoreComponent;
            _closeButton = closeComponent;
            _submitButton = submitButton;
            _header = header;

            _closeButton.OnClick.Add(() => _viewModel.CloseCommand.Execute(null));
        }

        protected override void OnShow()
        {
            ResetStartGameCommand();
        }

        private void SetupGameData()
        {
            var score = _scoreComponent.NumberValue;

            if (score == -1)
            {
                score = 11;
            }

            _gameDataProvider.SetGameData(new GameData
            {
                DifficultyType = _difficultyPanel.Value,
                PlayToScore = score,
                PlayerColor = _colorPanel.Value
            });
        }


        protected override void OnClose()
        {
            _closeButton.OnClick.Clear();
            _submitButton.OnClick.Clear();
            base.OnClose();
        }
    }
}

//Звук
//Пауза
//Настройка игры
//Конфигурация через файл
//Вид игры*
