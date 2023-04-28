using System;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Tween.Actions.Base;

namespace PhlegmaticOne.SharpTennis.Game.Common.Tween.Actions
{
    public class DelayedAction : TimedTweenAction 
    {
        public DelayedAction(Transform transform, float time, Action onComplete) : base(transform, time, onComplete) { }
        protected override void UpdateProtected(float deltaTime, float passedTime) { }
    }
}
