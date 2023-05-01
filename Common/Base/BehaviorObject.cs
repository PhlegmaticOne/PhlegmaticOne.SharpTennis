using System;
using PhlegmaticOne.SharpTennis.Game.Common.Tween;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;

namespace PhlegmaticOne.SharpTennis.Game.Common.Base
{
    public class BehaviorObject : Component
    {
        public bool Enabled { get; private set; } = true;

        public void UpdateBehavior()
        {
            if (Enabled)
            {
                Update();
            }
        }

        public virtual void OnCollisionEnter(Collider other) { }
        public virtual void OnCollisionStay(Collider other) { }
        public virtual void OnCollisionExit(Collider other) { }
        public virtual void Start() { }
        public virtual void OnDestroy() { }
        protected virtual void Update() { }
        protected static void Invoke(float time, Action action) => DoTween.ExecuteAfterTime(time, action);

        protected void DontDestroyOnLoad(GameObject gameObject) => gameObject.DestroyOnLoad = false;

        public void ChangeEnabled(bool enabled)
        {
            Enabled = enabled;
        }
    }
}
