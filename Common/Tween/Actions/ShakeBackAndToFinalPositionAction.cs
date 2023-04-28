using System;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Infrastructure;
using PhlegmaticOne.SharpTennis.Game.Common.Tween.Actions.Base;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Common.Tween.Actions
{
    public class ShakeBackAndToFinalPositionAction : TimedTweenAction
    {
        private readonly Transform _transform;
        private readonly Vector3 _deltaShake;
        private readonly Vector3 _deltaShakeFinal;
        private readonly float _halfTime;

        public ShakeBackAndToFinalPositionAction(Transform transform, Vector3 shake, Vector3 finalPosition,
            float duration, Action onComplete) : base(transform, duration, onComplete)
        {
            _transform = transform;
            _halfTime = duration / 2;
            _deltaShake = shake / (_halfTime / Time.DeltaT);
            _deltaShakeFinal = (finalPosition - (transform.Position + shake)) / (_halfTime / Time.DeltaT);
        }

        protected override void UpdateProtected(float deltaTime, float passedTime)
        {
            _transform.Move(passedTime <= _halfTime ? _deltaShake : _deltaShakeFinal);
        }
    }
}
