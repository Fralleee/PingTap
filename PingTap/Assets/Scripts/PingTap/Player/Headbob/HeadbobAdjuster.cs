using UnityEngine;

namespace Fralle.PingTap
{
	public class HeadbobAdjuster : MonoBehaviour
	{
		[SerializeField] HeadbobConfiguration configuration;
		HeadbobMaster headbob;

		void Start()
		{
			if (configuration)
			{
				headbob = GetComponentInParent<HeadbobMaster>();
				headbob.overrideConfguration = configuration;
			}

		}

		void OnDestroy()
		{
			if (configuration)
				headbob.overrideConfguration = null;
		}
	}
}
