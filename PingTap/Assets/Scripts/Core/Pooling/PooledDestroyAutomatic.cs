using UnityEngine;

namespace Fralle.Core.Pooling
{
	[RequireComponent(typeof(PooledObject))]
	public class PooledDestroyAutomatic : MonoBehaviour
	{
		AudioSource audioSource;
		new ParticleSystem particleSystem;

		void Awake()
		{
			audioSource = GetComponent<AudioSource>();
			particleSystem = GetComponent<ParticleSystem>();
		}

		void Update()
		{
			bool performDestroy = false;
			if (particleSystem)
				performDestroy = !particleSystem.IsAlive();

			if (audioSource)
				performDestroy = !audioSource.isPlaying;

			if (performDestroy)
				ObjectPool.Despawn(gameObject);
		}
	}
}
