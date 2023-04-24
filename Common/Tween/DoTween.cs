using System;
using PhlegmaticOne.SharpTennis.Game.Common.Tween.Actions;

namespace PhlegmaticOne.SharpTennis.Game.Common.Tween
{
    public static class DoTween
    {
        public static void ExecuteAfterTime(float time, Action action)
        {
            DoTweenManager.Instance.AddAction(new DelayedAction(time, action));
        }
    }
}
