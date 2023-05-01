using System;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Infrastructure;

namespace PhlegmaticOne.SharpTennis.Game.Common.Tween.Actions.Base
{
    public abstract class TimedTweenAction : ITweenAction
    {
        protected readonly float ExecutionTime;
        private readonly Action _onComplete;

        private float _passedTime;

        protected TimedTweenAction(Transform transform, float time, Action onComplete)
        {
            Transform = transform;
            ExecutionTime = time;
            _onComplete = onComplete;
        }

        public virtual void OnKill(Action action) => action?.Invoke();

        public bool IsFinished { get; private set; }

        public Transform Transform { get; }

        public void Update()
        {
            var delta = Time.DeltaT;
            _passedTime += delta;

            if (_passedTime >= ExecutionTime || IsFinished)
            {
                IsFinished = true;
                OnFinished();
                return;
            }

            UpdateProtected(delta, _passedTime);
        }
        protected float GetInterval() => ExecutionTime / Time.DeltaT;

        protected abstract void UpdateProtected(float deltaTime, float passedTime);
        protected virtual void OnFinishedProtected() { }

        private void OnFinished()
        {
            _onComplete?.Invoke();
            OnFinishedProtected();
        }
    }
}
