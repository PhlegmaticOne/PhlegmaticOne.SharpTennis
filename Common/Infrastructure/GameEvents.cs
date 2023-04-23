using System;
using System.Drawing;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Common.Infrastructure
{
    public static class GlobalEvents
    {
        public static event Action<SizeF> ScreenResized;
        public static event Action MouseClicked;
        public static event Action<Vector2> MouseMoved;
        public static void OnScreenResized(SizeF screenResized) => ScreenResized?.Invoke(screenResized);
        public static void OnMouseClicked() => MouseClicked?.Invoke();
        public static void OnMouseMoved(Vector2 relativeMove) => MouseMoved?.Invoke(relativeMove);
    }
}
