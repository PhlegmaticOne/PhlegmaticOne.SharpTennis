namespace PhlegmaticOne.SharpTennis.Game.Common.Sound.Models.Data
{
    public interface ISoundSettingsProvider
    {
        SoundSettings Settings { get; }
        void ForceSave();
    }
}
