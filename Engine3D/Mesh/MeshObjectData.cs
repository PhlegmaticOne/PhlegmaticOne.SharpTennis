using System;
using PhlegmaticOne.SharpTennis.Game.Engine3D.DirectX;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh.Structs;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using Buffer11 = SharpDX.Direct3D11.Buffer;

namespace PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh
{
    public class MeshObjectData : IDisposable
    {
        private Buffer11 _vertexBufferObject;
        private Buffer11 _indicesBufferObject;

        public MeshObjectData(VertexDataStruct[] vertices, uint[] indices, 
            PrimitiveTopology primitiveTopology, Material material,
            Buffer11 vertexBufferObject, Buffer11 indicesBufferObject, VertexBufferBinding vertexBufferBinding)
        {
            _vertexBufferObject = vertexBufferObject;
            _indicesBufferObject = indicesBufferObject;
            VertexBufferBinding = vertexBufferBinding;
            Vertices = vertices;
            Indices = indices;
            PrimitiveTopology = primitiveTopology;
            Material = material;
        }

        public static MeshObjectData Create(VertexDataStruct[] vertices, uint[] indices,
            PrimitiveTopology primitiveTopology, Material material, DirectX3DGraphics directX3DGraphics)
        {
            var vertexBufferObject = Buffer11.Create(directX3DGraphics.Device, BindFlags.VertexBuffer,
                vertices, Utilities.SizeOf<VertexDataStruct>() * vertices.Length);

            var vertexBufferBinding = new VertexBufferBinding(vertexBufferObject, 
                Utilities.SizeOf<VertexDataStruct>(), 0);

            var indicesBufferObject = Buffer11.Create(directX3DGraphics.Device, BindFlags.IndexBuffer,
                indices, Utilities.SizeOf<uint>() * indices.Length);

            return new MeshObjectData(vertices, indices, primitiveTopology, material, vertexBufferObject,
                indicesBufferObject, vertexBufferBinding);
        }

        public VertexDataStruct[] Vertices { get; }
        public uint[] Indices { get; }
        public PrimitiveTopology PrimitiveTopology { get; }
        public Material Material { get; }
        public Buffer11 IndicesBufferObject => _indicesBufferObject;
        public VertexBufferBinding VertexBufferBinding { get; }

        public void Dispose()
        {
            Utilities.Dispose(ref _indicesBufferObject);
            Utilities.Dispose(ref _vertexBufferObject);
            Utilities.Dispose(ref _indicesBufferObject);
        }
    }
}
