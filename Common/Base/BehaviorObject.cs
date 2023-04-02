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
        
        public virtual void Start() { }
        public virtual void OnDestroy() { }
        protected virtual void Update() { }

        public void ChangeEnabled(bool enabled)
        {
            Enabled = enabled;

            if (enabled)
            {
                OnEnable();
            }
            else
            {
                OnDisable();
            }
        }

        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }
    }
}
