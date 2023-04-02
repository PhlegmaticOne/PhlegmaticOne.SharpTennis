using System.Collections.Generic;
using System.Linq;
using PhlegmaticOne.SharpTennis.Game.Common.Base;

namespace PhlegmaticOne.SharpTennis.Game.Common.Render
{
    public class RenderSequence : BehaviorObject
    {
        private readonly List<IRenderer> _rendererSequence;

        public RenderSequence(IEnumerable<IRenderer> renderSequence)
        {
            _rendererSequence = renderSequence.ToList();
        }

        protected override void Update()
        {
            foreach (var renderer in _rendererSequence)
            {
                renderer.PreRender();
                renderer.BeginRender();
                renderer.Render();
            }

            _rendererSequence.Reverse();

            foreach (var renderer in _rendererSequence)
            {
                renderer.EndRender();
            }

            _rendererSequence.Reverse();
        }

        public override void OnDestroy()
        {
            foreach (var renderer in _rendererSequence)
            {
                renderer.Dispose();
            }
        }
    }
}
