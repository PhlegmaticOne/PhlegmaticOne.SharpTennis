using System;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Tween.Actions;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Common.Tween
{
    public static class DoTweenExtensions
    {
        public static void DoShake(this Transform transform, Vector3 shake, Vector3 finalPosition, float duration, Action onComplete = null)
        {
            DoTweenManager.Instance.AddAction(new ShakeBackAndToFinalPositionAction(transform, shake, finalPosition, duration, onComplete));
        }
    }
}
