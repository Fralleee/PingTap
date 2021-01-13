using UnityEngine;

namespace Fralle.Core.Pooling
{
	[RequireComponent(typeof(PooledObject))]
	public class PooledDestroyAutomatic : MonoBehaviour
	{
		AudioSource audioSource;
		ParticleSystem particles;

		void Awake()
		{
			audioSource = GetComponent<AudioSource>();
			particles = GetComponent<ParticleSystem>();
		}

		void Update()
		{
			bool performDestroy = false;
			if (particles)
				performDestroy = !particles.IsAlive();

			if (audioSource)
				performDestroy = !audioSource.isPlaying;

			if (performDestroy)
				ObjectPool.Despawn(gameObject);
		}
	}
}
