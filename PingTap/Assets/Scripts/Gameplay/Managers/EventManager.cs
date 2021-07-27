using System;
using System.Collections.Generic;

namespace Fralle.Gameplay
{
	public static class EventManager
	{
		static readonly Dictionary<Type, Action<GameEvent>> Events = new Dictionary<Type, Action<GameEvent>>();
		static readonly Dictionary<Delegate, Action<GameEvent>> EventLookups = new Dictionary<Delegate, Action<GameEvent>>();

		public static void AddListener<T>(Action<T> evt) where T : GameEvent
		{
			if (EventLookups.ContainsKey(evt))
				return;

			void NewAction(GameEvent e) => evt((T)e);
			EventLookups[evt] = NewAction;

			if (Events.TryGetValue(typeof(T), out Action<GameEvent> internalAction))
				Events[typeof(T)] = internalAction + NewAction;
			else
				Events[typeof(T)] = NewAction;
		}

		public static void RemoveListener<T>(Action<T> evt) where T : GameEvent
		{
			if (!EventLookups.TryGetValue(evt, out Action<GameEvent> action))
				return;
			if (Events.TryGetValue(typeof(T), out Action<GameEvent> tempAction))
			{
				tempAction -= action;
				if (tempAction == null)
					Events.Remove(typeof(T));
				else
					Events[typeof(T)] = tempAction;
			}

			EventLookups.Remove(evt);
		}

		public static void Broadcast(GameEvent evt)
		{
			if (Events.TryGetValue(evt.GetType(), out Action<GameEvent> action))
				action.Invoke(evt);
		}

		public static void Clear()
		{
			Events.Clear();
			EventLookups.Clear();
		}
	}
}
