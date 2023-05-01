using PhlegmaticOne.SharpTennis.Game.Common.Base.Scenes;
using PhlegmaticOne.SharpTennis.Game.Common.Commands;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
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
            var sceneBuilder = _sceneBuilderFactory.CreateSceneBuilder(_sceneBuilderFactory.Scenes.Menu);
            var scene = sceneBuilder.BuildScene();
            _sceneProvider.ChangeScene(scene);
        }
    }
}
