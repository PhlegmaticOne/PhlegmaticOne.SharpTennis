namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements
{
    public class ScoreSystem
    {
        public ScoreText PlayerText { get; set; }
        public ScoreText EnemyText { get; set; }

        public void AddScoreToPlayer(int score)
        {
            PlayerText.AddScore(score);
        }

        public void AddScoreToEnemy(int score)
        {
            EnemyText.AddScore(score);
        }
    }
}
