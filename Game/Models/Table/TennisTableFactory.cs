using System.Collections.Generic;
using System.Linq;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Rigid;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Base;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Table
{
    public class TennisTableFactory : IFactory<TennisTable>
    {
        private readonly MeshLoader _meshLoader;
        private readonly TextureMaterialsProvider _textureMaterialsProvider;

        public TennisTableFactory(MeshLoader meshLoader, TextureMaterialsProvider textureMaterialsProvider)
        {
            _meshLoader = meshLoader;
            _textureMaterialsProvider = textureMaterialsProvider;
        }

        public TennisTable Create(Transform transform)
        {
            var table = _meshLoader.LoadFbx("assets\\models\\tennis_table.fbx", _textureMaterialsProvider.DefaultTexture);
            CreateTableTop(table);
            CreateTableNet(table);
            CreateTableBase(table);

            foreach (var meshComponent in table)
            {
                meshComponent.Transform.SetRotation(new Vector3(0, -90, 0));
            }

            return new TennisTable(table);
        }

        private void CreateTableBase(List<MeshComponent> allMeshes)
        {
            var top = allMeshes[2];
            var corner2 = top.MeshObjectData.Vertices[5].position;
            var halfWidth = corner2.Y;
            var halfHeight = corner2.X;
            var tableBase = allMeshes.Last();
            var height = tableBase.MeshObjectData.Vertices.Max(x => x.position.Z);
            var offset = new Vector3(-halfHeight + 3, -height, halfWidth - 2);
            tableBase.Transform.SetPosition(offset);
            tableBase.Transform.Rotate(new Vector3(0, -90, 0));
        }

        private void CreateTableNet(List<MeshComponent> allMeshes)
        {
            var net1 = allMeshes[3];
            var net = allMeshes[4];

            var top = allMeshes[2];
            var corner = top.MeshObjectData.Vertices[5].position;
            var halfWidth = corner.Y;

            var min = net.MeshObjectData.Vertices[1].position;
            Swap(ref min);
            min.Z *= -1;
            min.X = -0.5f;
            var max = net.MeshObjectData.Vertices[3].position;
            Swap(ref max);
            max.X = 0.5f;
            max.Z *= -1;

            var offset = new Vector3(0, 1.3f, -halfWidth - 2);
            var collider = new BoxCollider3D((Vector3)min + offset, (Vector3)max + offset);
            net.GameObject.AddComponent(collider);
            net.Transform.Move(offset);
            net1.Transform.Move(offset);
            net.Transform.SetRotation(new Vector3(0, -90, 0));
            net1.Transform.SetRotation(new Vector3(0, -90, 0));
        }

        private void CreateTableTop(List<MeshComponent> allMeshes)
        {
            var top = allMeshes[2];
            top.GameObject.AddComponent(CreateTopCollider(top));
            top.GameObject.AddComponent(new RigidBody3D(Vector3.Zero));
            top.GameObject.Name = "TopTable";
        }

        private BoxCollider3D CreateTopCollider(MeshComponent top)
        {
            var corner1 = top.MeshObjectData.Vertices[1].position;
            Swap(ref corner1, -2.5f);
            var corner2 = top.MeshObjectData.Vertices[5].position;
            Swap(ref corner2, 2.5f);
            corner2.Y = -0.3f;
            var boxCollider = new BoxCollider3D((Vector3)corner1, (Vector3)corner2);
            return boxCollider;
        }

        private void Swap(ref Vector4 a)
        {
            var z = a.Z;
            a.Z = a.Y;
            a.Y = z;
        }

        private void Swap(ref Vector4 a, float increaseZBy)
        {
            var z = a.Z;
            a.Z = a.Y;
            a.Z += increaseZBy;
            a.X += increaseZBy;
            a.Y = z;
        }
    }
}
