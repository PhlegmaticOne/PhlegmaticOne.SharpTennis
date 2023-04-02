using System;

namespace PhlegmaticOne.SharpTennis.Game.Common.Render
{
    public interface IRenderer : IDisposable
    {
        void PreRender();
        void BeginRender();
        void Render();
        void EndRender();
    }
}
