namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements
{
    public class ScoreSystem
    {
        private readonly ScoreText _playerText;
        private readonly ScoreText _enemyText;

        public ScoreSystem(ScoreText playerText, ScoreText enemyText)
        {
            _playerText = playerText;
            _enemyText = enemyText;
        }

        public void AddScoreToPlayer(int score)
        {
            _playerText.AddScore(score);
        }

        public void AddScoreToEnemy(int score)
        {
            _enemyText.AddScore(score);
        }
    }
}
