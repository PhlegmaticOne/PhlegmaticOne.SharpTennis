using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Common.Base
{
    public static class VectorExtensions
    {
        public static Vector3 Normalized(this Vector3 v)
        {
            var copy = v;
            copy.Normalize();
            return copy;
        }
    }
}
