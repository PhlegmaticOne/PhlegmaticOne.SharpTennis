using System.Threading.Tasks;
using PhlegmaticOne.SharpTennis.Game.Common.Base.Scenes;
using PhlegmaticOne.SharpTennis.Game.Common.Commands;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Loading;
using PhlegmaticOne.SharpTennis.Game.Game.Scenes.Base;

namespace PhlegmaticOne.SharpTennis.Game.Game.Commands
{
    public class ExitToMenuCommand : ICommand
    {
        private readonly PopupSystem _popupSystem;
        private readonly SceneProvider _sceneProvider;
        private readonly ISceneBuilderFactory<TennisGameScenes> _sceneBuilderFactory;

        public ExitToMenuCommand(PopupSystem popupSystem, SceneProvider sceneProvider,
            ISceneBuilderFactory<TennisGameScenes> sceneBuilderFactory)
        {
            _popupSystem = popupSystem;
            _sceneProvider = sceneProvider;
            _sceneBuilderFactory = sceneBuilderFactory;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            _popupSystem.CloseAll();
            _popupSystem.SpawnPopup<LoadingPopup>();
            ChangeSceneAsync();
        }

        private async void ChangeSceneAsync()
        {
            var result = await Task.WhenAll(Task.Run(async () =>
            {
                await Task.Delay(2000);
                var sceneBuilder = _sceneBuilderFactory.CreateSceneBuilder(_sceneBuilderFactory.Scenes.Menu);
                _popupSystem.CloseAll();
                return sceneBuilder.BuildScene();
            }));

            var scene = result[0];
            _sceneProvider.ChangeScene(scene);
        }
    }
}
