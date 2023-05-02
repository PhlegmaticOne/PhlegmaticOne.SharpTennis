using System;
using System.IO;
using Newtonsoft.Json;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Game.Player.Data
{
    public class JsonPlayerDataProvider : IPlayerDataProvider
    {
        private const string FilePath = @"Assets\Data\player_data.json";
        public PlayerData PlayerData { get; }
        public event Action<PlayerData> Changed;

        public JsonPlayerDataProvider() => PlayerData = LoadData();

        public void ForceSave()
        {
            var json = JsonConvert.SerializeObject(PlayerData);
            File.WriteAllText(FilePath, json);
            Changed?.Invoke(PlayerData);
        }

        private PlayerData LoadData()
        {
            if (File.Exists(FilePath) == false)
            {
                return new PlayerData
                {
                    Name = "player"
                };
            }
            var json = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<PlayerData>(json);
        }
    }
}
