using System.Runtime.InteropServices;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexDataStruct
    {
        public Vector4 position;
        public Vector4 normal;
        public Vector4 color;
        public Vector2 texCoord;
    }
}