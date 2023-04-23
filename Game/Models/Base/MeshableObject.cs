using System.Collections.Generic;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Base
{
    public class MeshableObject : BehaviorObject
    {
        public List<MeshComponent> Meshes { get; } = new List<MeshComponent>();
        public void AddMeshes(params MeshComponent[] meshes) => Meshes.AddRange(meshes);
        public void AddMeshes(IEnumerable<MeshComponent> meshes) => Meshes.AddRange(meshes);
    }
}
