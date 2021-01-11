using UnityEngine;

namespace Fralle.Core.Pooling
{
	[RequireComponent(typeof(PooledObject))]
	public class PooledDestroyDelayed : MonoBehaviour
	{
		[SerializeField] float delay;

		float timer;

		void OnEnable()
		{
			timer = delay;
		}

		void Update()
		{
			timer -= Time.deltaTime;
			if (timer <= 0)
				ObjectPool.Despawn(gameObject);
		}
	}
}
