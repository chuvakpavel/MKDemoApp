using Android.Media;
using MKDemoApp.Droid.Services;
using MKDemoApp.Exceptions;
using MKDemoApp.Services;
using System;
using System.IO;
using Xamarin.Forms;
using Stream = System.IO.Stream;
using Uri = Android.Net.Uri;

[assembly: Dependency(typeof(AudioPlayerService))]

namespace MKDemoApp.Droid.Services
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public sealed class AudioPlayerService : IAudioPlayerService
    {
        public event EventHandler PlaybackEnded;

        #region Properties

        public double AudioDuration => _player?.Duration / 1000d ?? throw new AudioPlayerIsNotInitializedException();

        public double CurrentSecond =>
            _player?.CurrentPosition / 1000d ?? throw new AudioPlayerIsNotInitializedException();

        public float Volume
        {
            get => _volume;
            set
            {
                if (_player == null)
                {
                    throw new AudioPlayerIsNotInitializedException();
                }

                _volume = value;

                value = Math.Max(0f, value);
                value = Math.Min(1f, value);

                _player.SetVolume(value, value);
            }
        }

        private float _volume = 0.5f;

        public bool IsPlaying => _player?.IsPlaying ?? throw new AudioPlayerIsNotInitializedException();

        public bool IsLooped
        {
            get => _player?.Looping ?? throw new AudioPlayerIsNotInitializedException();
            set
            {
                if (_player == null)
                {
                    throw new AudioPlayerIsNotInitializedException();
                }

                _player.Looping = value;
            }
        }

        #endregion Properties

        public void Init(Stream audioStream)
        {
            _player.Reset();

            DeleteCachedFile(_cachedFilePath);

            //cache to the file system
            _cachedFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                $"cache{DateTime.Now.Ticks}.wav");

            var fileStream = File.Create(_cachedFilePath);
            audioStream.CopyTo(fileStream);
            fileStream.Close();

            try
            {
                _player.SetDataSource(_cachedFilePath);
            }
            catch
            {
                try
                {
                    var context = Android.App.Application.Context;

                    _player.SetDataSource(context,
                        Uri.Parse(Uri.Encode(_cachedFilePath)) ??
                        throw new ArgumentException($"{_cachedFilePath} is not valid."));
                }
                catch
                {
                    //TODO exception ignored
                }
            }

            _player.Prepare();
        }

        public void Play()
        {
            if (_player == null)
            {
                throw new AudioPlayerIsNotInitializedException();
            }

            if (IsPlaying)
            {
                Pause();

                Seek(0d);
            }

            _player.Start();
        }

        public void Stop()
        {
            if (!IsPlaying)
            {
                return;
            }

            Pause();
            Seek(0);
        }

        public void Pause()
        {
            if (_player == null)
            {
                throw new AudioPlayerIsNotInitializedException();
            }

            _player.Pause();
        }

        public void Seek(double second)
        {
            if (_player == null)
            {
                throw new AudioPlayerIsNotInitializedException();
            }

            _player.SeekTo((int) second * 1000);
        }

        private void OnPlaybackEnded(object sender, EventArgs e)
        {
            PlaybackEnded?.Invoke(sender, e);

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
            {
                return;
            }

            _player.SeekTo(0);

            _player.Stop();

            _player.Prepare();
        }

        private MediaPlayer _player;

        private string _cachedFilePath;

        private bool _isDisposed;

        public AudioPlayerService()
        {
            _player = new MediaPlayer();
            _player.Completion += OnPlaybackEnded;
        }

        ~AudioPlayerService()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        private static void DeleteCachedFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            try
            {
                File.Delete(path);
            }
            catch
            {
                //TODO exception ignored
            }
        }

        private void Dispose(bool disposing)
        {
            if (_isDisposed || _player == null)
            {
                return;
            }

            if (disposing)
            {
                Stop();

                if (_player != null)
                {
                    _player.Completion -= OnPlaybackEnded;
                    _player.Release();
                    _player.Dispose();
                    _player = null;
                }

                DeleteCachedFile(_cachedFilePath);

                _cachedFilePath = string.Empty;
            }

            _isDisposed = true;
        }
    }
}