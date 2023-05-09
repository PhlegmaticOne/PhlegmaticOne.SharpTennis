using PhlegmaticOne.SharpTennis.Game.Game.Models.Base;
using SharpDX;
using System.Globalization;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Game
{
    public class GameData
    {
        public DifficultyType DifficultyType { get; set; }
        public ColorType PlayerColor { get; set; }
        public int PlayToScore { get; set; }
        public int TimeInMinutes { get; set; }
        public GameType GameType { get; set; }
        public string CustomColor { get; set; }

        public Color ParsedColor()
        {
            var color = int.Parse(CustomColor + "FF", NumberStyles.HexNumber);
            return Color.FromAbgr(color);
        }
    }

    public enum GameType
    {
        Score,
        Time
    }
}
