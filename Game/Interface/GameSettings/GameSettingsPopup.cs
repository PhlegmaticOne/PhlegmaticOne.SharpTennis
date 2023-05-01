using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.GameSettings
{
    public class GameSettingsPopup : PopupBase
    {
        private readonly GameDataProvider _gameDataProvider;
        private readonly GameSettingsViewModel _viewModel;

        private SelectablePanel<ColorType> _colorPanel;
        private SelectablePanel<DifficultyType> _difficultyPanel;
        private InputNumberSelectableElement _scoreComponent;
        private ButtonComponent _closeButton;
        private ButtonComponent _submitButton;


        public GameSettingsPopup(GameDataProvider gameDataProvider, GameSettingsViewModel viewModel)
        {
            _gameDataProvider = gameDataProvider;
            _viewModel = viewModel;
        }


        public void Setup(SelectablePanel<ColorType> colorsPanel, 
            SelectablePanel<DifficultyType> difficultyPanel,
            InputNumberSelectableElement scoreComponent,
            ButtonComponent closeComponent,
            ButtonComponent submitButton)
        {
            _colorPanel = colorsPanel;
            _difficultyPanel = difficultyPanel;
            _scoreComponent = scoreComponent;
            _closeButton = closeComponent;
            _submitButton = submitButton;

            _closeButton.OnClick.Add(() => _viewModel.CloseCommand.Execute(null));
            _submitButton.OnClick.Add(() =>
            {
                SetupGameData();
                _viewModel.StartGameCommand.Execute(null);
            });
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
        }
    }
}

//Звук
//Пауза
//Настройка игры
//Конфигурация через файл
//Вид игры*
