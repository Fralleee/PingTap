using UnityEngine;

namespace Fralle.Core.Infrastructure
{
	public class PooledDestroyAutomatic : MonoBehaviour
	{
		AudioSource audioSource;
		ParticleSystem particleSystem;

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
			{
				Debug.Log(gameObject.name + " is to be destroyed");
				ObjectPool.Destroy(gameObject);
			}

		}
	}
}
