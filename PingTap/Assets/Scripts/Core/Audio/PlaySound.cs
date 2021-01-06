using Fralle.Core.Extensions;
using UnityEngine;

namespace Fralle.Core.Audio
{
	public class PlaySound : MonoBehaviour
	{
		[SerializeField] [Range(0f, 3f)] float pitchRandomMultiplier = 1.2f;

		AudioSource audioSource;

		void Awake()
		{
			audioSource = GetComponent<AudioSource>();
		}

		void OnEnable()
		{
			if (!pitchRandomMultiplier.EqualsWithTolerance(1f))
			{
				if (Random.value < .5)
					audioSource.pitch *= Random.Range(1 / pitchRandomMultiplier, 1);
				else
					audioSource.pitch *= Random.Range(1, pitchRandomMultiplier);
			}
			audioSource.Play();
		}
	}
}
