using System;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Game.Player.Data
{
    public interface IPlayerDataProvider
    {
        PlayerData PlayerData { get; }
        event Action<PlayerData> Changed; 
        void ForceSave();
    }
}
