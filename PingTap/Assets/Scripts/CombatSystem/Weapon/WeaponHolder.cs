using UnityEngine;

namespace CombatSystem
{
	public class WeaponHolder : MonoBehaviour
	{
		[SerializeField] bool clearWeaponsOnAwake = true;

		void Awake()
		{
			if (clearWeaponsOnAwake)
			{
				foreach (Transform child in transform)
				{
					Destroy(child.gameObject);
				}
			}
		}
	}
}
