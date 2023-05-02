using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game.Player;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game.Player.Data;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements
{
    public class ScoreSystem : BehaviorObject
    {
        private readonly IPlayerDataProvider _playerDataProvider;
        public ScoreText PlayerText { get; set; }
        public ScoreText EnemyText { get; set; }

        public ScoreSystem(IPlayerDataProvider playerDataProvider)
        {
            _playerDataProvider = playerDataProvider;
            _playerDataProvider.Changed += PlayerDataProviderOnChanged;
        }

        private void PlayerDataProviderOnChanged(PlayerData obj)
        {
            PlayerText.ChangePreScoreText(obj.Name);
        }

        public override void OnDestroy()
        {
            _playerDataProvider.Changed -= PlayerDataProviderOnChanged;
        }

        public void AddScore(int score, RacketType ballBouncedFromType)
        {
            if (ballBouncedFromType == RacketType.Player)
            {
                PlayerText.AddScore(score);
            }

            if (ballBouncedFromType == RacketType.Enemy)
            {
                EnemyText.AddScore(score);
            }
        }
    }
}
