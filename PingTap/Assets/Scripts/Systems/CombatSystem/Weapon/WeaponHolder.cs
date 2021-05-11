using System.Linq;
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
				string[] stringArray = { "Weapon Camera", "FPS" };
				foreach (Transform child in transform)
				{
					if (!stringArray.Any(child.name.Contains))
					{
						Destroy(child.gameObject);
					}
				}
			}
		}
	}
}
