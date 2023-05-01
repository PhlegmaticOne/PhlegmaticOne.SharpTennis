namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Game
{
    public class GameDataProvider
    {
        public GameData GameData { get; private set; }

        public void SetGameData(GameData gameData)
        {
            GameData = gameData;
        }
    }
}
