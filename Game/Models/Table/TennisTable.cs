﻿using System.Collections.Generic;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Table
{
    public class TennisTable : BehaviorObject
    {
        public List<MeshComponent> Meshes { get; }

        public TennisTable(List<MeshComponent> meshes)
        {
            Meshes = meshes;
        }
    }
}