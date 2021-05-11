using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.PingTap
{
	[CreateAssetMenu(menuName = "Settings/Headbob")]
	public class HeadbobConfiguration : ScriptableObject
	{
		[Range(0f, 1f)] public float BobbingSpeed = 0.08f;
		[Range(0f, 3f)] public float CameraBobbingAmount = 0.05f;
		[Range(0f, 3f)] public float WeaponRotationAmount = 1.75f;
	}
}
