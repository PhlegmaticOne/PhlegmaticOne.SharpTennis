using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements
{
    public class ScoreSystem : Component
    {
        public ScoreText PlayerText { get; set; }
        public ScoreText EnemyText { get; set; }

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
