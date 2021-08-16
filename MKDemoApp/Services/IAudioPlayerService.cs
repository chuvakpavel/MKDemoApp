using System;
using System.IO;

namespace MKDemoApp.Services
{
    public interface IAudioPlayerService : IDisposable
    {
        event EventHandler PlaybackEnded;

        double AudioDuration { get; }

        double CurrentSecond { get; }

        float Volume { get; set; }

        bool IsPlaying { get; }

        bool IsLooped { get; set; }

        void Init(Stream audioStream);

        void Play();

        void Pause();

        void Stop();

        void Seek(double second = 0);
    }
}