using System.Drawing;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements
{
    public class ScoreText : RectComponent
    {
        private readonly TextComponent _textComponent;
        private readonly string _preScoreText;
        private int _score;

        public ScoreText(TextComponent textComponent, string preScoreText, bool isPlayerScore)
        {
            RectTransform = textComponent.RectTransform;
            IsPlayerScore = isPlayerScore;
            _textComponent = textComponent;
            _preScoreText = preScoreText;
            var fontSize = textComponent.TextFormatData.FontSize;
            textComponent.RectTransform.Size = new SizeF(300, fontSize);
            ResetScore();
        }

        public bool IsPlayerScore { get; }

        public void AddScore(int score)
        {
            var newScore = _score + score;
            UpdateScoreText(newScore);
        }

        public void ResetScore()
        {
            UpdateScoreText(0);
        }

        private void UpdateScoreText(int score)
        {
            _score = score;
            _textComponent.Text = FormatScoreText(score);
        }

        private string FormatScoreText(int score) => _preScoreText + " : " + score;
    }
}
