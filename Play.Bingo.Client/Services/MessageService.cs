using System;
using System.Collections;
using System.Linq;

namespace Play.Bingo.Client.Services
{
    /// <summary> A very poor implementation of <see cref="IMessageService" />. </summary>
    public class MessageService : IMessageService
    {
        private readonly ArrayList _handlers = new ArrayList();

        /// <summary> <see cref="IMessageService.Publish{T}" />. </summary>
        public void Publish<T>(T message)
        {
            foreach (var handler in _handlers.OfType<Action<T>>())
            {
                handler(message);
            }
        }

        /// <summary> <see cref="IMessageService.Subscribe{T}" />. </summary>
        public void Subscribe<T>(Action<T> handler)
        {
            _handlers.Add(handler);
        }

        public void Unsubscribe<T>(Action<T> handler)
        {
            _handlers.Remove(handler);
        }
    }
}