using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;

namespace PhlegmaticOne.SharpTennis.Game.Engine2D.Base
{
    public interface IElementsCreator
    {
        Bitmap CreateImage(string fileName);
        SolidColorBrush CreateBrush(RawColor4 color);
        TextFormat CreateTextFormat(TextFormatData textFormatData);
    }
}
