using PhlegmaticOne.SharpTennis.Game.Game.Models.Base;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Game
{
    public class GameData
    {
        public DifficultyType DifficultyType { get; set; }
        public ColorType PlayerColor { get; set; }
        public int PlayToScore { get; set; }
    }
}
