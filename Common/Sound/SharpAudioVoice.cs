using System;
using System.IO;
using System.Threading;
using SharpDX.Multimedia;
using SharpDX.XAudio2;

namespace PhlegmaticOne.SharpTennis.Game.Common.Sound
{
    public class SharpAudioVoice : IDisposable
    {
        private readonly AudioBuffer _buffer;
        private readonly SoundStream _stream;
        private Thread _checkThread;

        private bool _isPlaying;
        public SourceVoice Voice { get; }

        public event Action<SharpAudioVoice> Stopped;

        public SharpAudioVoice(SharpAudioDevice device, string filename)
        {
            _isPlaying = false;
            _stream = new SoundStream(File.OpenRead(filename));

            var waveFormat = _stream.Format;
            Voice = new SourceVoice(device.Device, waveFormat);

            _buffer = new AudioBuffer
            {
                Stream = _stream.ToDataStream(),
                AudioBytes = (int)_stream.Length,
                Flags = BufferFlags.EndOfStream
            };

        }

        public void Play()
        {
            if (_isPlaying) return;
            Voice.SubmitSourceBuffer(_buffer, _stream.DecodedPacketsInfo);
            Voice.Start();
            _isPlaying = true;
            _checkThread = new Thread(Check);
            _checkThread.Start();

        }

        private void Check()
        {
            while (Voice.State.BuffersQueued > 0)
            {
                Thread.Sleep(10);
            }
            Voice.Stop();
            Voice.FlushSourceBuffers();
            _isPlaying = false;
            Stopped?.Invoke(this);
        }

        public void Stop()
        {
            if (!_isPlaying) return;
            Voice.ExitLoop();
            Voice.Stop();
            Voice.FlushSourceBuffers();
            _isPlaying = false;
            _checkThread.Abort();
        }

        public void Dispose()
        {
            Voice.DestroyVoice();
            Voice.Dispose();
            _stream.Dispose();
            _buffer.Stream.Dispose();
        }
    }
}
