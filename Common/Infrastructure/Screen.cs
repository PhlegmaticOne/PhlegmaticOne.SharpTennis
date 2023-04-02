using System.Drawing;

namespace PhlegmaticOne.SharpTennis.Game.Common.Infrastructure
{
    public static class Screen
    {
        public static float Width;
        public static float Height;
        public static SizeF Size;

        public static void Initialize(SizeF size)
        {
            Width = size.Width;
            Height = size.Height;
            Size = size;
        }
    }
}
