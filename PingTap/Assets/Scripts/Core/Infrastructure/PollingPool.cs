using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fralle.Core.Infrastructure
{
	public abstract class PollingPool<T> where T : Component
	{
		private readonly T prefab;

		private readonly Queue<T> pool = new Queue<T>();
		private readonly LinkedList<T> inuse = new LinkedList<T>();
		private readonly Queue<LinkedListNode<T>> nodePool = new Queue<LinkedListNode<T>>();

		private int lastCheckFrame = -1;

		protected PollingPool(T prefab, int preWarm = 0)
		{
			this.prefab = prefab;
			if (preWarm > 0)
				foreach (var item in Enumerable.Range(0, preWarm).Select((i, index) => Object.Instantiate(prefab)))
				{
					item.gameObject.SetActive(true);
					pool.Enqueue(item);
				}
		}

		private void CheckInUse()
		{
			var node = inuse.First;
			while (node != null)
			{
				var current = node;
				node = node.Next;

				if (!IsActive(current.Value))
				{
					current.Value.gameObject.SetActive(false);
					pool.Enqueue(current.Value);
					inuse.Remove(current);
					nodePool.Enqueue(current);
				}
			}
		}

		protected T Get()
		{
			T item;

			if (lastCheckFrame != Time.frameCount)
			{
				lastCheckFrame = Time.frameCount;
				CheckInUse();
			}

			if (pool.Count == 0)
				item = Object.Instantiate(prefab);
			else
				item = pool.Dequeue();

			if (nodePool.Count == 0)
				inuse.AddLast(item);
			else
			{
				var node = nodePool.Dequeue();
				node.Value = item;
				inuse.AddLast(node);
			}

			item.gameObject.SetActive(true);

			return item;
		}

		protected abstract bool IsActive(T component);
	}
}
