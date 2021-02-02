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
			if (particles && particles.IsAlive())
				return;


			if (audioSource && audioSource.isPlaying)
				return;

			ObjectPool.Despawn(gameObject);
		}
	}
}
