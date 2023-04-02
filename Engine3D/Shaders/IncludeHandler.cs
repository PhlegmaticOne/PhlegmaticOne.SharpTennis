using System;
using System.IO;
using System.Windows.Forms;
using SharpDX.D3DCompiler;

namespace PhlegmaticOne.SharpTennis.Game.Engine3D.Shaders
{
    public class IncludeHandler : Include
    {
        private Stream _stream;

        public IDisposable Shadow { get; set; }


        public void Close(Stream stream)
        {
            _stream.Dispose();
            _stream = null;
        }

        public void Dispose() => _stream?.Dispose();

        public Stream Open(IncludeType type, string fileName, Stream parentStream)
        {
            var path = Application.StartupPath;
            var files = new DirectoryInfo(path).GetFiles(fileName, SearchOption.AllDirectories);
            return new FileStream(files[0].FullName, FileMode.Open);
        }
    }
}
