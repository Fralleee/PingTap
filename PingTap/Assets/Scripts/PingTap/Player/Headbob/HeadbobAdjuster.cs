using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.PingTap
{
	public class HeadbobAdjuster : MonoBehaviour
	{
		[SerializeField] HeadbobConfiguration configuration;
		Headbob headbob;

		void Start()
		{
			if (configuration)
			{
				headbob = GetComponentInParent<Headbob>();
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
