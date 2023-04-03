using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.WIC;
using SharpDX;
using System.Collections.Generic;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.DirectX;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh.Structs;

namespace PhlegmaticOne.SharpTennis.Game.Engine3D
{
    public class MeshLoader
    {
        private readonly DirectX3DGraphics _directX3DGraphics;
        private readonly SamplerState _pointSampler;
        private readonly ImagingFactory _imagingFactory;
        private readonly SampleDescription _sampleDescription;

        public MeshLoader(DirectX3DGraphics directX3DGraphics, SamplerState pointSampler)
        {
            _sampleDescription = new SampleDescription(1, 0);
            _directX3DGraphics = directX3DGraphics;
            _pointSampler = pointSampler;
            _imagingFactory = new ImagingFactory();
        }

        public List<MeshComponent> LoadFbx(string filename, Texture textur)
        {
            var importer = new Assimp.AssimpContext();
            importer.SetConfig(new Assimp.Configs.NormalSmoothingAngleConfig(66.0f));
            var model = importer.ImportFile(filename, Assimp.PostProcessPreset.TargetRealTimeMaximumQuality);

            var meshes = new List<MeshComponent>();

            foreach (var mesh in model.Meshes)
            {
                var mat = model.Materials[mesh.MaterialIndex];
                Material material;
                var vertices = new List<VertexDataStruct>();

                if (mat.GetMaterialTexture(Assimp.TextureType.Diffuse, 0, out Assimp.TextureSlot slot))
                {
                    var ambient = mat.ColorAmbient;
                    var diffuse = mat.ColorDiffuse;
                    var emissive = mat.ColorEmissive;
                    var secular = mat.ColorSpecular;
                    var texturePath = slot.FilePath;

                    while (texturePath[0] == '.')
                    {
                        texturePath = texturePath.Substring(3, texturePath.Length - 3);
                    }

                    texturePath = string.Concat("assets\\", texturePath);

                    Texture texture = LoadTextureFromFile(texturePath, false);

                    material = new Material(texture,
                        new Vector3(emissive.R, emissive.G, emissive.B),
                        new Vector3(ambient.R, ambient.G, ambient.B),
                        new Vector3(diffuse.R, diffuse.G, diffuse.B),
                        new Vector3(secular.R, secular.G, secular.B),
                        8.0f);
                    try
                    {
                        for (int i = 0; i < mesh.VertexCount; i++)
                        {
                            vertices.Add(new VertexDataStruct 
                            {
                                position = new Vector4(mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z, 1.0f),
                                normal = new Vector4(mesh.Normals[i].X, mesh.Normals[i].Y, mesh.Normals[i].Z, 1.0f),
                                color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                                texCoord = new Vector2(mesh.TextureCoordinateChannels[0][i].X, 1f - mesh.TextureCoordinateChannels[0][i].Y)
                            });
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
                else
                {
                    var ambient = mat.ColorAmbient;
                    var diffuse = mat.ColorDiffuse;
                    var emissive = mat.ColorEmissive;
                    var secular = mat.ColorSpecular;

                    material = new Material(textur,
                        new Vector3(emissive.R, emissive.G, emissive.B),
                        new Vector3(ambient.R, ambient.G, ambient.B),
                        new Vector3(diffuse.R, diffuse.G, diffuse.B),
                        new Vector3(secular.R, secular.G, secular.B),
                        8.0f);

                    for (int i = 0; i < mesh.VertexCount; i++)
                    {
                        vertices.Add(new VertexDataStruct
                        {
                            position = new Vector4(mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z, 1.0f),
                            normal = new Vector4(mesh.Normals[i].X, mesh.Normals[i].Y, mesh.Normals[i].Z, 1.0f),
                            color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        });
                    }
                }

                var indices = new List<uint>();
                try
                {
                    for (int i = 0; i < mesh.FaceCount; i++)
                    {
                        indices.Add((uint)mesh.Faces[i].Indices[2]);
                        indices.Add((uint)mesh.Faces[i].Indices[1]);
                        indices.Add((uint)mesh.Faces[i].Indices[0]);
                    }
                }
                catch
                {
                    continue;
                }

                var meshData = MeshObjectData.Create(vertices.ToArray(), indices.ToArray(), 
                    PrimitiveTopology.TriangleList, material, _directX3DGraphics);

                var go = new GameObject();
                var meshComponent = new MeshComponent(meshData);
                go.AddComponent(meshComponent);
                meshes.Add(meshComponent);
            }

            return meshes;
        }


        public Texture LoadTextureFromFile(string fileName, bool generateMips, int mipLevels = -1)
        {
            var decoder = new BitmapDecoder(_imagingFactory, fileName, DecodeOptions.CacheOnDemand);
            var bitmapFirstFrame = decoder.GetFrame(0);
            Utilities.Dispose(ref decoder);

            var formatConverter = new FormatConverter(_imagingFactory);
            formatConverter.Initialize(bitmapFirstFrame, PixelFormat.Format32bppRGBA,
                BitmapDitherType.None, null, 0.0f, BitmapPaletteType.Custom);

            var stride = formatConverter.Size.Width * 4;
            var buffer = new DataStream(formatConverter.Size.Height * stride, true, true);
            formatConverter.CopyPixels(stride, buffer);

            var width = formatConverter.Size.Width;
            var height = formatConverter.Size.Height;

            var texture2DDescription = new Texture2DDescription
            {
                Width = width,
                Height = height,
                MipLevels = generateMips ? 0 : 1,
                ArraySize = 1,
                Format = Format.R8G8B8A8_UNorm,
                SampleDescription = _sampleDescription,
                Usage = ResourceUsage.Default,
                BindFlags = generateMips ? BindFlags.ShaderResource | BindFlags.RenderTarget : BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = generateMips ? ResourceOptionFlags.GenerateMipMaps : ResourceOptionFlags.None
            };

            Texture2D textureObject;

            if (generateMips)
            {
                textureObject = new Texture2D(_directX3DGraphics.Device, texture2DDescription);
            }
            else
            {
                var dataRectangle = new DataRectangle(buffer.DataPointer, stride);
                textureObject = new Texture2D(_directX3DGraphics.Device, texture2DDescription, dataRectangle);
            }

            var shaderResourceViewDescription = new ShaderResourceViewDescription()
            {
                Dimension = ShaderResourceViewDimension.Texture2D,
                Format = Format.R8G8B8A8_UNorm,
                Texture2D = new ShaderResourceViewDescription.Texture2DResource
                {
                    MostDetailedMip = 0,
                    MipLevels = (generateMips ? mipLevels : 1)
                }
            };

            var shaderResourceView = new ShaderResourceView(_directX3DGraphics.Device, textureObject, shaderResourceViewDescription);

            if (generateMips)
            {
                var dataBox = new DataBox(buffer.DataPointer, stride, 1);
                _directX3DGraphics.DeviceContext.UpdateSubresource(dataBox, textureObject, 0);
                _directX3DGraphics.DeviceContext.GenerateMips(shaderResourceView);
            }

            Utilities.Dispose(ref formatConverter);

            return new Texture(textureObject, shaderResourceView, width, height, _pointSampler);
        }
    }
}
