using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device11 = SharpDX.Direct3D11.Device;
using Buffer11 = SharpDX.Direct3D11.Buffer;
using System;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Infrastructure;
using PhlegmaticOne.SharpTennis.Game.Common.Render;
using PhlegmaticOne.SharpTennis.Game.Engine3D.DirectX;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh.Structs;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Shaders;
using Assimp;
using PhlegmaticOne.SharpTennis.Game.Common.Base.Scenes;
using LightSourceType = PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh.Structs.LightSourceType;
using Scene = PhlegmaticOne.SharpTennis.Game.Common.Base.Scenes.Scene;

namespace PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh
{
    public class MeshRenderer : BehaviorObject, IRenderer
    {
        private const int MaxLights = 8;
        private const int LightSourceTypeCount = (int)Structs.LightSourceType.Spot + 1;

        private readonly string[] _lightClassVariableNames = {
            "baseLight",
            "directinalLight",
            "pointLight",
            "spotLight"
        };

        private readonly DirectX3DGraphics _directX3DGraphics;
        private readonly SceneProvider _sceneProvider;
        private readonly Device11 _device;
        private readonly DeviceContext _deviceContext;
        private readonly int[] _lightVariableOffsets = new int[MaxLights];
        private VertexShader _vertexShader;
        private PixelShader _pixelShader;
        private ShaderSignature _shaderSignature;
        private InputLayout _inputLayout;
        private PerFrameConstantBuffer _perFrameConstantBuffer;
        private PerObjectConstantBuffer _perObjectConstantBuffer;
        private MaterialConstantBuffer _materialConstantBuffer;
        private Material _currentMaterial;
        private Texture _currentTexture;
        private SamplerState _anisotropicSampler;
        private SamplerState _linearSampler;
        private ClassLinkage _pixelShaderClassLinkage;
        private ClassLinkage _vertexShaderClassLinkage;
        private Buffer11 _illuminationPropertiesBufferObject;
        private ClassInstance[] _lightInterfaces;
        private ClassInstance[] _lightInstances;
        private int _lightInterfaceCount;
        private IlluminationProperties _illuminationProperties;

        public SamplerState PointSampler { get; }

        public MeshRenderer(DirectX3DGraphics directX3DGraphics, MeshRendererData meshRendererData,
            SceneProvider sceneProvider)
        {
            var pixelData = meshRendererData.PixelShaderInfo;
            var vertexData = meshRendererData.VertexShaderInfo;

            _directX3DGraphics = directX3DGraphics;
            _sceneProvider = sceneProvider;
            _device = _directX3DGraphics.Device;
            _deviceContext = _directX3DGraphics.DeviceContext;

            _pixelShaderClassLinkage = new ClassLinkage(_device);
            _vertexShaderClassLinkage = new ClassLinkage(_device);


            var pixelShaderByteCode =
                ShaderBytecode.CompileFromFile(pixelData.ShaderPath, pixelData.EntryPoint, pixelData.Profile,
                    ShaderFlags.Debug | ShaderFlags.SkipOptimization, EffectFlags.None, null, new IncludeHandler());

            _pixelShader = new PixelShader(_device, pixelShaderByteCode, _pixelShaderClassLinkage);

            InitializeIllumination(pixelShaderByteCode);

            var vertexShaderByteCode =
                ShaderBytecode.CompileFromFile(vertexData.ShaderPath, vertexData.EntryPoint, vertexData.Profile, ShaderFlags.None, EffectFlags.None, null, new IncludeHandler());

            _vertexShader = new VertexShader(_device, vertexShaderByteCode, _vertexShaderClassLinkage);
            Utilities.Dispose(ref _vertexShaderClassLinkage);

            var inputElements = new[]
            {
                new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
                new InputElement("NORMAL", 0, Format.R32G32B32A32_Float, 16, 0),
                new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 32, 0),
                new InputElement("TEXCOORD", 0, Format.R32G32_Float, 48, 0)
            };

            _shaderSignature = ShaderSignature.GetInputSignature(vertexShaderByteCode);

            _inputLayout = new InputLayout(_device, _shaderSignature, inputElements);

            Utilities.Dispose(ref vertexShaderByteCode);
            Utilities.Dispose(ref pixelShaderByteCode);

            _deviceContext.InputAssembler.InputLayout = _inputLayout;


            var samplerStateDescription = new SamplerStateDescription
            {
                Filter = Filter.Anisotropic,
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
                AddressW = TextureAddressMode.Clamp,
                MipLodBias = 0,
                MaximumAnisotropy = 16,
                ComparisonFunction = Comparison.Never,
                BorderColor = new SharpDX.Mathematics.Interop.RawColor4(1.0f, 1.0f, 1.0f, 1.0f),
                MinimumLod = 0,
                MaximumLod = float.MaxValue
            };

            _anisotropicSampler = new SamplerState(_directX3DGraphics.Device, samplerStateDescription);

            samplerStateDescription.Filter = Filter.MinMagMipLinear;
            _linearSampler = new SamplerState(_directX3DGraphics.Device, samplerStateDescription);

            samplerStateDescription.Filter = Filter.MinMagMipPoint;
            PointSampler = new SamplerState(_directX3DGraphics.Device, samplerStateDescription);

            CreateConstantBuffers();
            CreateIllumination();
        }

        private void CreateIllumination()
        {
            _illuminationProperties = new IlluminationProperties
            {
                globalAmbient = new Vector3(0.02f)
            };
            var lightSource = new LightSource
            {
                lightSourceType = LightSourceType.Base,
                ConstantAttenuation = 0.01f,
                color = Vector3.Zero
            };
            for (var i = 0; i < MaxLights; i++)
            {
                _illuminationProperties[i] = lightSource;
            }

            _illuminationProperties[0] = GenerateDefaultLight();
        }

        private LightSource GenerateDefaultLight()
        {
            var lightSource = new LightSource
            {
                lightSourceType = LightSourceType.Directional,
                ConstantAttenuation = 0.01f,
                color = new Vector3(0.99f, 0.84f, 0.69f),
                direction = Vector3.Normalize(new Vector3(2f, -2.0f, -1.0f))
            };

            return lightSource;
        }


        public void BeginRender()
        {
            _directX3DGraphics.ClearBuffers(Color.Black);
        }

        public void PreRender()
        {
            var scene = _sceneProvider.Scene;

            if (scene?.Camera == null)
            {
                return;
            }

            var camera = scene.Camera;
            UpdatePerFrameConstantBuffers(Time.PassedTime);
            _illuminationProperties.eyePosition = (Vector4)camera.Transform.Position;
            UpdateIllumination();
        }


        public void Render()
        {
            var scene = _sceneProvider.Scene;

            if (scene?.Camera == null)
            {
                return;
            }

            var camera = scene.Camera;
            var viewMatrix = camera.GetViewMatrix();
            var projectionMatrix = camera.GetProjectionMatrix();

            foreach (var meshComponent in scene.GetComponents<MeshComponent>())
            {
                UpdatePerObjectConstantBuffers(meshComponent.Transform.GetWorldMatrix(),
                    viewMatrix, projectionMatrix, 0);
                RenderMeshObject(meshComponent);
            }
        }


        public void EndRender()
        {
            _directX3DGraphics.SwapChain.Present(1, PresentFlags.Restart);
        }


        private void UpdatePerFrameConstantBuffers(float time)
        {
            _perFrameConstantBuffer.Update(time);
        }

        private void UpdatePerObjectConstantBuffers(Matrix world, Matrix view, Matrix projection, int timeScaling)
        {
            _perObjectConstantBuffer.Update(world, view, projection, timeScaling);
        }

        private void SetTexture(Texture texture)
        {
            if (_currentTexture != texture && texture != null)
            {
                _deviceContext.PixelShader.SetShaderResource(0, texture.ShaderResourceView);
                _deviceContext.PixelShader.SetSampler(0, texture.SamplerState);
                _currentTexture = texture;
            }
        }

        private void SetMaterial(Material material)
        {
            if (_currentMaterial != material)
            {
                SetTexture(material.Texture);
                _currentMaterial = material;
                _materialConstantBuffer.Update(material.MaterialProperties);
            }
        }

        private void RenderMeshObject(MeshComponent meshObject)
        {
            var meshData = meshObject.MeshObjectData;
            SetMaterial(meshData.Material);
            _deviceContext.InputAssembler.PrimitiveTopology = meshData.PrimitiveTopology;
            _deviceContext.InputAssembler.SetVertexBuffers(0, meshData.VertexBufferBinding);
            _deviceContext.InputAssembler.SetIndexBuffer(meshData.IndicesBufferObject, Format.R32_UInt, 0);
            _deviceContext.VertexShader.Set(_vertexShader);
            _deviceContext.PixelShader.Set(_pixelShader, _lightInterfaces);
            _deviceContext.DrawIndexed(meshData.Indices.Length, 0, 0);
        }

        
        public void UpdateIllumination()
        {
            _deviceContext.MapSubresource(_illuminationPropertiesBufferObject,
                MapMode.WriteDiscard, SharpDX.Direct3D11.MapFlags.None, out var dataStream);
            dataStream.Write(_illuminationProperties);
            _deviceContext.UnmapSubresource(_illuminationPropertiesBufferObject, 0);
            _deviceContext.PixelShader.SetConstantBuffer(1, _illuminationPropertiesBufferObject);

            for (var i = 0; i < MaxLights; ++i)
            {
                _lightInterfaces[_lightVariableOffsets[i]] = _lightInstances[(int)_illuminationProperties[i].lightSourceType];
            }
        }

        public void Dispose()
        {
            _perObjectConstantBuffer.Dispose();
            _perFrameConstantBuffer.Dispose();
            _materialConstantBuffer.Dispose();
            Utilities.Dispose(ref _linearSampler);
            Utilities.Dispose(ref _anisotropicSampler);
            Utilities.Dispose(ref _inputLayout);
            Utilities.Dispose(ref _shaderSignature);
            Utilities.Dispose(ref _illuminationPropertiesBufferObject);

            for (var i = 0; i < LightSourceTypeCount; ++i)
            {
                Utilities.Dispose(ref _lightInstances[i]);
            }

            _lightInterfaces = null;
            Utilities.Dispose(ref _pixelShader);
            Utilities.Dispose(ref _vertexShader);
            Utilities.Dispose(ref _pixelShaderClassLinkage);

            
        }

        private void InitializeIllumination(CompilationResult pixelShaderByteCode)
        {
            var pixelShaderReflection = new ShaderReflection(pixelShaderByteCode);
            _lightInterfaceCount = pixelShaderReflection.InterfaceSlotCount;

            if (_lightInterfaceCount != MaxLights)
            {
                throw new IndexOutOfRangeException("Light interfaces count");
            }

            _lightInterfaces = new ClassInstance[_lightInterfaceCount];

            var shaderVariableLights = pixelShaderReflection.GetVariable("lights");

            for (var i = 0; i < MaxLights; ++i)
            {
                _lightVariableOffsets[i] = shaderVariableLights.GetInterfaceSlot(i);
            }

            _lightInstances = new ClassInstance[LightSourceTypeCount];

            for (var i = 0; i < LightSourceTypeCount; ++i)
            {
                _lightInstances[i] = _pixelShaderClassLinkage.GetClassInstance(_lightClassVariableNames[i], 0);
            }

            Utilities.Dispose(ref shaderVariableLights);
            Utilities.Dispose(ref pixelShaderReflection);
        }


        private void CreateConstantBuffers()
        {
            _perFrameConstantBuffer = new PerFrameConstantBuffer(_device, _deviceContext, _deviceContext.VertexShader, 0, 0);
            _perObjectConstantBuffer = new PerObjectConstantBuffer(_device, _deviceContext, _deviceContext.VertexShader, 0, 1);
            _materialConstantBuffer = new MaterialConstantBuffer(_device, _deviceContext, _deviceContext.PixelShader, 0, 0);
            _illuminationPropertiesBufferObject = new Buffer11(_device,
                Utilities.SizeOf<IlluminationProperties>(), ResourceUsage.Dynamic,
                BindFlags.ConstantBuffer, CpuAccessFlags.Write, ResourceOptionFlags.None, 0);
        }
    }
}
