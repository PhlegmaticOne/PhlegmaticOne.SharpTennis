﻿using System;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using Device11 = SharpDX.Direct3D11.Device;
using Resource = SharpDX.Direct3D11.Resource;

namespace PhlegmaticOne.SharpTennis.Game.Engine3D.DirectX
{
    public class DirectX3DGraphics : IDisposable
    {
        private readonly RenderForm _renderForm;
        private readonly SwapChainDescription _swapChainDescription;

        private Device11 _device;
        private SwapChain _swapChain;
        private DeviceContext _deviceContext;
        private RasterizerState _rasterizerStage;
        private RenderTargetView _renderTargetView;
        private Factory _factory;
        private Texture2D _backBuffer;
        private Texture2D _depthStencilBuffer;
        private DepthStencilView _depthStencilView;
        private bool _isFullScreen;
        private Texture2DDescription _depthStencilBufferDescription;
        public Device11 Device => _device;
        public SwapChain SwapChain => _swapChain;
        public DeviceContext DeviceContext => _deviceContext;
        public Texture2D BackBuffer => _backBuffer;

        public bool IsFullScreen
        {
            get => _isFullScreen;
            set
            {
                if(value != _isFullScreen)
                {
                    _isFullScreen = value;
                    _swapChain.SetFullscreenState(_isFullScreen, null);
                }
            }
        }

        public DirectX3DGraphics(RenderForm renderForm)
        {
            _renderForm = renderForm;

            Configuration.EnableObjectTracking = true;

            var sampleDescription = new SampleDescription(4, (int)StandardMultisampleQualityLevels.StandardMultisamplePattern);

            _swapChainDescription = new SwapChainDescription
            {
                BufferCount = 1,
                ModeDescription = new ModeDescription(_renderForm.Width, _renderForm.Height, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = renderForm.Handle,
                SampleDescription = sampleDescription,
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            Device11.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.BgraSupport,
                _swapChainDescription, out _device, out _swapChain);

            _deviceContext = _device.ImmediateContext;

            _rasterizerStage = new RasterizerState(_device, new RasterizerStateDescription()
            {
                FillMode = FillMode.Solid,
                CullMode = CullMode.Back,
                IsFrontCounterClockwise = true,
                IsMultisampleEnabled = true,
                IsAntialiasedLineEnabled = true,
                IsDepthClipEnabled = true
            });

            _deviceContext.Rasterizer.State = _rasterizerStage;
            _factory = _swapChain.GetParent<Factory>();

            _factory.MakeWindowAssociation(_renderForm.Handle, WindowAssociationFlags.IgnoreAll);
            _depthStencilBufferDescription = new Texture2DDescription
            {
                Format = Format.D32_Float_S8X24_UInt,
                ArraySize = 1,
                MipLevels = 1,
                Width = _renderForm.ClientSize.Width,
                Height = _renderForm.ClientSize.Height,
                SampleDescription = sampleDescription,
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            };
        }

        public void Resize()
        {
            Utilities.Dispose(ref _depthStencilView);
            Utilities.Dispose(ref _depthStencilBuffer);
            Utilities.Dispose(ref _renderTargetView);
            Utilities.Dispose(ref _backBuffer);

            _swapChain.ResizeBuffers(_swapChainDescription.BufferCount,
                _renderForm.ClientSize.Width, _renderForm.ClientSize.Height,
                Format.Unknown, SwapChainFlags.None);

            _backBuffer = Resource.FromSwapChain<Texture2D>(_swapChain, 0);

            _renderTargetView = new RenderTargetView(_device, _backBuffer);

            _depthStencilBufferDescription.Width = _renderForm.ClientSize.Width;
            _depthStencilBufferDescription.Height = _renderForm.ClientSize.Height;
            _depthStencilBuffer = new Texture2D(_device, _depthStencilBufferDescription);

            _depthStencilView = new DepthStencilView(_device, _depthStencilBuffer);

            _deviceContext.Rasterizer.SetViewport(
                new Viewport(0, 0, _renderForm.ClientSize.Width, _renderForm.ClientSize.Height, 0f, 1f));
            _deviceContext.OutputMerger.SetTargets(_depthStencilView, _renderTargetView);
        }

        public void ClearBuffers(Color backgroundColor)
        {
            _deviceContext.ClearDepthStencilView(_depthStencilView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1f, 0 );
            _deviceContext.ClearRenderTargetView(_renderTargetView, backgroundColor );
        }

        public void Dispose()
        {
            Utilities.Dispose(ref _depthStencilView);
            Utilities.Dispose(ref _depthStencilBuffer);
            Utilities.Dispose(ref _renderTargetView);
            Utilities.Dispose(ref _backBuffer);
            Utilities.Dispose(ref _factory);
            Utilities.Dispose(ref _rasterizerStage);
            Utilities.Dispose(ref _deviceContext);
            Utilities.Dispose(ref _swapChain);
            Utilities.Dispose(ref _device);
        }
    }
}
