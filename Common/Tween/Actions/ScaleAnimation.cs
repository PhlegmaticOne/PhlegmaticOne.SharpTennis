using System;
using PhlegmaticOne.SharpTennis.Game.Common.Infrastructure;
using PhlegmaticOne.SharpTennis.Game.Common.Tween.Actions.Base;
using SharpDX;
using Transform = PhlegmaticOne.SharpTennis.Game.Common.Base.Transform;

namespace PhlegmaticOne.SharpTennis.Game.Common.Tween.Actions
{
    public class ScaleAnimation : TimedTweenAction
    {
        private readonly Transform _transform;
        private readonly Vector3 _from;
        private readonly Vector3 _to;
        private readonly bool _doBack;
        private readonly int _coeff;
        private bool _first;

        public ScaleAnimation(Transform transform, Vector3 from, Vector3 to, float time, bool doBack, Action onComplete) :
            base(transform, time, onComplete)
        {
            _first = true;
            _transform = transform;
            _from = from;
            _to = to;
            _coeff = _doBack ? 2 : 1;
            _doBack = doBack;
        }

        public override void OnKill(Action onKill)
        {
            _transform.SetScale(_from);
            base.OnKill(onKill);
        }

        protected override void UpdateProtected(float deltaTime, float passedTime)
        {
            if (_first)
            {
                _transform.SetScale(_from);
                _first = false;
            }

            var delta = _coeff * (_to - _from) / GetInterval();


            if (_doBack == false)
            {
                _transform.ScaleBy(delta);
                return;
            }

            _transform.ScaleBy(passedTime >= ExecutionTime / 2  ? -delta : delta);
        }
    }
}
