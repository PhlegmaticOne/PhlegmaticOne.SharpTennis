using System;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Tween.Actions;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Common.Tween
{
    public static class DoTweenExtensions
    {
        public static void DoKill(this Transform transform, Action onKill = null)
        {
            DoTweenManager.Instance.KillTweensWithTransform(transform, onKill);
        }

        public static void DoShake(this Transform transform, Vector3 shake, Vector3 finalPosition, float duration, Action onComplete = null)
        {
            DoTweenManager.Instance.AddAction(new ShakeBackAndToFinalPositionAction(transform, shake, finalPosition, duration, onComplete));
        }

        public static void DoScale(this Transform transform, Vector3 from, Vector3 to, float duration, bool doBack,
            Action onComplete = null)
        {
            DoTweenManager.Instance.AddAction(new ScaleAnimation(transform, from, to, duration, doBack, onComplete));
        }
    }
}
