using System;
using System.Drawing;
using System.Windows.Forms;
using PhlegmaticOne.SharpTennis.Game.Game;
using SharpDX.Direct3D;
using SharpDX.Windows;
using Device11 = SharpDX.Direct3D11.Device;

namespace PhlegmaticOne.SharpTennis.Game
{
    internal static class Program
    {
        [STAThread]
        public static void Main()
        {
            if(Device11.GetSupportedFeatureLevel() != FeatureLevel.Level_11_0)
            {
                MessageBox.Show("DirectX11 not Supported");
                return;
            }

            var renderForm = new RenderForm
            {
                IsFullscreen = true,
                MinimumSize = new Size(960, 540),
                WindowState = FormWindowState.Maximized
            };

            using (var gameRunner = new GameRunner(renderForm))
            {
                gameRunner.Run();
            }
        }
    }
}
