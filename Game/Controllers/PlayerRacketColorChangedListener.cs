using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game.Player;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game.Player.Data;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Racket;
using SharpDX;
using System.Globalization;

namespace PhlegmaticOne.SharpTennis.Game.Game.Controllers
{
    public class PlayerRacketColorChangedListener : BehaviorObject
    {
        private readonly IPlayerDataProvider _playerDataProvider;
        private PlayerRacket _playerRacket;

        public PlayerRacketColorChangedListener(IPlayerDataProvider playerDataProvider)
        {
            _playerDataProvider = playerDataProvider;
            _playerDataProvider.Changed += PlayerDataProviderOnChanged;
        }

        private void PlayerDataProviderOnChanged(PlayerData obj)
        {
            _playerRacket.Color(ParseColor(obj.Color));
        }

        public void Setup(PlayerRacket playerRacket)
        {
            _playerRacket = playerRacket;
        }

        public override void OnDestroy()
        {
            _playerDataProvider.Changed -= PlayerDataProviderOnChanged;
        }

        private Color ParseColor(string hex)
        {
            var color = int.Parse(hex + "FF", NumberStyles.HexNumber);
            return Color.FromAbgr(color);
        }
    }
}
