using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace PhlegmaticOne.SharpTennis.Game.Engine2D.Popups
{
    public class PopupSystem
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly CanvasManager _canvasManager;
        private readonly List<PopupBase> _popups;

        public IReadOnlyList<PopupBase> Popups => _popups;

        public PopupSystem(IServiceProvider serviceProvider, CanvasManager canvasManager)
        {
            _serviceProvider = serviceProvider;
            _canvasManager = canvasManager;
            _popups = new List<PopupBase>();
        }

        public T SpawnPopup<T>() where T : PopupBase
        {
            var popup = _serviceProvider.GetRequiredService<PopupFactory<T>>().CreatePopup();
            _canvasManager.AddCanvas(popup.Canvas, true, true);
            _popups.Add(popup);
            popup.Show();
            return popup;
        }

        public void CloseAll()
        {
            while (_popups.Count != 0)
            {
                CloseLastPopup();
            }
        }

        public void CloseLastPopup(bool disableCursor = false)
        {
            var last = _popups.Last();
            _popups.Remove(last);
            _canvasManager.Remove(last.Canvas);
            last.Close();

            if (_popups.Count == 0 || disableCursor)
            {
                _canvasManager.ChangeCursorEnabled(false);
            }
        }
    }
}
