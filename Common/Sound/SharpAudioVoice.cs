using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SharpDX.Multimedia;
using SharpDX.XAudio2;

namespace PhlegmaticOne.SharpTennis.Game.Common.Sound
{
    public class SharpAudioVoice : IDisposable
    {
        private readonly AudioBuffer _buffer;
        private readonly SoundStream _stream;
        private readonly SourceVoice _voice;

        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;
        private bool _isPlaying;

        public SharpAudioVoice(SharpAudioDevice device, string filename)
        {
            _isPlaying = false;
            InitializeCancellation();

            _stream = new SoundStream(File.OpenRead(filename));
            _voice = new SourceVoice(device.Device, _stream.Format);
            _buffer = new AudioBuffer
            {
                Stream = _stream.ToDataStream(),
                AudioBytes = (int)_stream.Length,
                Flags = BufferFlags.EndOfStream
            };
        }

        private void InitializeCancellation()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
        }


        public void Play()
        {
            if (_isPlaying)
            {
                Stop();
            }

            _voice.SubmitSourceBuffer(_buffer, _stream.DecodedPacketsInfo);
            _voice.Start();
            _isPlaying = true;
            PlaySoundAsync();
        }

        public void Stop()
        {
            _voice.ExitLoop();
            _voice.Stop();
            _voice.FlushSourceBuffers();
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            InitializeCancellation();
            _isPlaying = false;
        }

        private void PlaySoundAsync()
        {
            Task.Factory.StartNew(async () =>
            {
                while (_voice.State.BuffersQueued > 0)
                {
                    await Task.Delay(100, _cancellationToken);

                    if (_cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }
                }
                _voice.Stop();
                _voice.FlushSourceBuffers();
                _isPlaying = false;
            }, _cancellationToken);
        }

        public void Dispose()
        {
            _voice.DestroyVoice();
            _voice.Dispose();
            _stream.Dispose();
            _buffer.Stream.Dispose();
        }
    }
}
