using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eternia.Game.Events
{
    public interface IEventHandler<T>
    {
        void Handle(T @event);
    }

    public class Event
    {
        private static List<object> eventHandlers = new List<object>();

        public static void Subscribe<T>(IEventHandler<T> handler)
        {
            if (!eventHandlers.Contains(handler))
                eventHandlers.Add(handler);
        }

        public static void Raise<T>(T @event)
        {
            foreach (var handler in eventHandlers)
            {
                var eventHandler = handler as IEventHandler<T>;
                if (eventHandler != null)
                {
                    eventHandler.Handle(@event);
                }
            }
        }
    }
}
