#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Fralle
{
	[CustomEditor(typeof(PlayerAttack)), CanEditMultipleObjects]
	public class PlayerAttackEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			var playerAttack = (PlayerAttack)target;

			if (GUILayout.Button("Equip Weapon"))
				playerAttack.EquipFirstWeaponInList();

			if (GUILayout.Button("Remove Weapon"))
				playerAttack.RemoveWeapon();

			base.OnInspectorGUI();

		}
	}
}
#endif
