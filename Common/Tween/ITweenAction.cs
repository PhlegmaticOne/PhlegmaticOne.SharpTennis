using System;
using PhlegmaticOne.SharpTennis.Game.Common.Base;

namespace PhlegmaticOne.SharpTennis.Game.Common.Tween
{
    public interface ITweenAction
    {
        Transform Transform { get; }
        void Update();
        void OnKill(Action action);
        bool IsFinished { get; }
    }
}
