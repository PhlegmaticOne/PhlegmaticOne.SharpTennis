using PhlegmaticOne.SharpTennis.Game.Game.Models.Base;

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
    }

    public enum GameType
    {
        Score,
        Time
    }
}
