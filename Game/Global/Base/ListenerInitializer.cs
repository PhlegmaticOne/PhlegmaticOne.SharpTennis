using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Input;

namespace PhlegmaticOne.SharpTennis.Game.Game.Global.Base
{
    public abstract class ListenerInitializer : BehaviorObject
    {
        protected readonly GlobalInputListener GlobalInputListener;

        protected ListenerInitializer(GlobalInputListener globalInputListener) => 
            GlobalInputListener = globalInputListener;

        public abstract void Initialize();
    }
}
