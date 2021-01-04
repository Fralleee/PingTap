using CombatSystem.Combat.Damage;
using Fralle.Targeting;
using UnityEngine;

namespace Fralle.AI
{
	public class EnemyAdapter : MonoBehaviour
	{
		DamageController damageController;
		TargetController targetController;

		void Awake()
		{
			targetController = GetComponentInChildren<TargetController>();
			damageController = GetComponentInChildren<DamageController>();
			damageController.OnReceiveAttack += DamageController_OnReceiveAttack;
		}

		void DamageController_OnReceiveAttack(DamageController dc, DamageData dd)
		{
			targetController.RaycastHit(1.5f);
		}

		void OnDestroy()
		{
			damageController.OnReceiveAttack -= DamageController_OnReceiveAttack;
		}
	}
}
