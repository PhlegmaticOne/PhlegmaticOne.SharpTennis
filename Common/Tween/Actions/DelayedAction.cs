using System;
using PhlegmaticOne.SharpTennis.Game.Common.Tween.Actions.Base;

namespace PhlegmaticOne.SharpTennis.Game.Common.Tween.Actions
{
    public class DelayedAction : TimedTweenAction 
    {
        public DelayedAction(float time, Action onComplete) : base(time, onComplete) { }
        protected override void UpdateProtected(float deltaTime, float passedTime) { }
    }
}
