using System;

namespace MKDemoApp.Exceptions
{
    public sealed class AudioPlayerIsNotInitializedException : Exception
    {
        public AudioPlayerIsNotInitializedException(
            string message = "Player isn't initialized. Use AudioService.Load(Stream audioStream)") : base(message)
        {
        }
    }
}