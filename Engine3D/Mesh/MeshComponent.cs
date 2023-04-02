using PhlegmaticOne.SharpTennis.Game.Common.Base;

namespace PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh
{
    public class MeshComponent : Component
    {
        public MeshObjectData MeshObjectData { get; }

        public MeshComponent(MeshObjectData meshObjectData)
        {
            MeshObjectData = meshObjectData;
        }
    }
}
