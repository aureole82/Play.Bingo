using System;

namespace Play.Bingo.Client.Services
{
    /// <summary> Accepts and distributes messages routed by their types. </summary>
    public interface IMessageService
    {
        /// <summary> Sends the message to everyone who's interested. </summary>
        void Publish<T>(T message);

        /// <summary> Registers the handler of the interested subscriber. </summary>
        void Subscribe<T>(Action<T> handler);

        /// <summary> Oh, the subscriber isn't interested anymore. Get rid of him. </summary>
        void Unsubscribe<T>(Action<T> handler);
    }
}