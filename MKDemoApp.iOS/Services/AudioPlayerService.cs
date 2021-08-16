using AVFoundation;
using Foundation;
using MKDemoApp.Exceptions;
using MKDemoApp.iOS.Services;
using MKDemoApp.Services;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(AudioPlayerService))]

namespace MKDemoApp.iOS.Services
{
    public sealed class AudioPlayerService : IAudioPlayerService
    {
        public event EventHandler PlaybackEnded;

        #region Properties

        public double AudioDuration => _player?.Duration ?? throw new AudioPlayerIsNotInitializedException();

        public double CurrentSecond => _player?.CurrentTime ?? throw new AudioPlayerIsNotInitializedException();

        public float Volume
        {
            get => _player?.Volume ?? throw new AudioPlayerIsNotInitializedException();
            set
            {
                if (_player == null)
                {
                    throw new AudioPlayerIsNotInitializedException();
                }

                value = Math.Max(0f, value);
                value = Math.Min(1f, value);

                _player.Volume = value;
                _player.Pan = 0f;
            }
        }

        public bool IsPlaying => _player?.Playing ?? throw new AudioPlayerIsNotInitializedException();

        public bool IsLooped
        {
            get => _isLooped;
            set
            {
                if (_player == null)
                {
                    throw new AudioPlayerIsNotInitializedException();
                }

                _isLooped = value;

                _player.NumberOfLoops = _isLooped ? -1 : 0;
            }
        }

        private bool _isLooped;

        #endregion Properties

        public void Init(Stream audioStream)
        {
            DeletePlayer();

            var data = NSData.FromStream(audioStream);

            _player = AVAudioPlayer.FromData(data);
            _player.FinishedPlaying += OnPlaybackEnded;

            _player.PrepareToPlay();
        }

        public void Play()
        {
            if (_player == null)
            {
                throw new AudioPlayerIsNotInitializedException();
            }

            if (_player.Playing)
            {
                _player.CurrentTime = 0d;
            }
            else
            {
                _player.Play();
            }
        }

        public void Pause()
        {
            if (_player == null)
            {
                throw new AudioPlayerIsNotInitializedException();
            }

            _player.Pause();
        }

        public void Stop()
        {
            if (_player == null)
            {
                throw new AudioPlayerIsNotInitializedException();
            }

            _player.Stop();

            Seek();
        }

        public void Seek(double second = 0d)
        {
            if (_player == null)
            {
                throw new AudioPlayerIsNotInitializedException();
            }

            _player.CurrentTime = second;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        private AVAudioPlayer _player;

        private bool _isDisposed;

        ~AudioPlayerService()
        {
            Dispose(false);
        }

        private void OnPlaybackEnded(object sender, AVStatusEventArgs e) => PlaybackEnded?.Invoke(sender, e);

        private void DeletePlayer()
        {
            Stop();

            if (_player == null)
            {
                return;
            }

            _player.FinishedPlaying -= OnPlaybackEnded;
            _player.Dispose();
            _player = null;
        }

        private void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                DeletePlayer();
            }

            _isDisposed = true;
        }
    }
}