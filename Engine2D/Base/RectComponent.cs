using System;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Transform;

namespace PhlegmaticOne.SharpTennis.Game.Engine2D.Base
{
    public abstract class RectComponent : Component, IDisposable
    {
        public bool Enabled { get; set; } = true;
        public RectTransform RectTransform { get => (RectTransform)Transform; set => Transform = value; }
        public virtual void HandleClick() { }
        public virtual void Dispose() { }
    }
}
