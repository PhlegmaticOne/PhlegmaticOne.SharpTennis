using System.Threading.Tasks;
using PhlegmaticOne.SharpTennis.Game.Common.Base.Scenes;
using PhlegmaticOne.SharpTennis.Game.Common.Commands;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Game;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Loading;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game;
using PhlegmaticOne.SharpTennis.Game.Game.Scenes.Base;

namespace PhlegmaticOne.SharpTennis.Game.Game.Commands
{
    public class StartGameCommand : ICommand
    {
        private readonly PopupSystem _popupSystem;
        private readonly SceneProvider _sceneProvider;
        private readonly ISceneBuilderFactory<TennisGameScenes> _sceneBuilder;
        private readonly GameRunner<TennisGameScenes> _gameRunner;
        private readonly GameDataProvider _gameDataProvider;

        public StartGameCommand(PopupSystem popupSystem,
            SceneProvider sceneProvider,
            ISceneBuilderFactory<TennisGameScenes> sceneBuilder,
            GameRunner<TennisGameScenes> gameRunner,
            GameDataProvider gameDataProvider)
        {
            _popupSystem = popupSystem;
            _sceneProvider = sceneProvider;
            _sceneBuilder = sceneBuilder;
            _gameRunner = gameRunner;
            _gameDataProvider = gameDataProvider;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            _popupSystem.CloseAll();
            _popupSystem.SpawnPopup<LoadingPopup>();
            LoadSceneAsync();
        }

        private async void LoadSceneAsync()
        {
            var result = await Task.WhenAll(Task.Run(async () =>
            {
                await Task.Delay(2000);
                var sceneBuilder = _sceneBuilder.CreateSceneBuilder(_sceneBuilder.Scenes.Game);
                return sceneBuilder.BuildScene();
            }));

            var scene = result[0];
            _sceneProvider.ChangeScene(scene);
            _popupSystem.CloseAll();
            var popup = _popupSystem.SpawnPopup<GamePopup>();
            popup.SetupGameData(_gameDataProvider.GameData);
            _gameRunner.ForceResize();
        }
    }
}
