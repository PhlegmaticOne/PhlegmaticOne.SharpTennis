using System.Collections.Generic;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Table
{
    public class TennisTable : BehaviorObject
    {
        private readonly TableTopPart _tableTopPart;
        private readonly TableNet _tableNet;
        public List<MeshComponent> Meshes { get; }

        public TennisTable(List<MeshComponent> meshes, 
            TableTopPart tableTopPart, TableNet tableNet)
        {
            _tableTopPart = tableTopPart;
            _tableNet = tableNet;
            Meshes = meshes;
        }
    }
}