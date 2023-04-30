using System.Windows.Forms;
using PhlegmaticOne.SharpTennis.Game.Common.Base.Scenes;
using PhlegmaticOne.SharpTennis.Game.Engine2D;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Menu;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Scenes.Base;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Win
{
    public class WinPopup : PopupBase
    {
        private readonly PopupSystem _popupSystem;
        private readonly ISceneBuilderFactory<TennisGameScenes> _sceneBuilderFactory;
        private readonly SceneProvider _sceneProvider;
        private readonly GameRunner<TennisGameScenes> _gameRunner;
        private ButtonComponent _continueButton;
        private ButtonComponent _exitButton;
        private TextComponent _winText;

        public WinPopup(PopupSystem popupSystem,
            ISceneBuilderFactory<TennisGameScenes> sceneBuilderFactory,
            SceneProvider sceneProvider,
            GameRunner<TennisGameScenes> gameRunner)
        {
            _popupSystem = popupSystem;
            _sceneBuilderFactory = sceneBuilderFactory;
            _sceneProvider = sceneProvider;
            _gameRunner = gameRunner;
        }


        public void Setup(ButtonComponent continueButton, ButtonComponent exitButton, TextComponent winText)
        {
            _continueButton = continueButton;
            _exitButton = exitButton;
            _winText = winText;

            _continueButton.OnClick.Add(ContinueGame);
            _exitButton.OnClick.Add(ExitGame);
        }

        public void SetWinner(RacketType racket)
        {
            _winText.Text = "Winner: " + racket;
        }

        private void ExitGame()
        {
            _popupSystem.CloseAll();
            var sceneBuilder = _sceneBuilderFactory.CreateSceneBuilder(_sceneBuilderFactory.Scenes.Menu);
            var scene = sceneBuilder.BuildScene();
            _sceneProvider.ChangeScene(scene);
            //_gameRunner.ForceResize();
        }

        private void ContinueGame()
        {

        }
    }
}
