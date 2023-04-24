namespace PhlegmaticOne.SharpTennis.Game.Common.Tween
{
    public interface ITweenAction
    {
        void Update();
        bool IsFinished { get; }
    }
}
