using UnityEngine;

namespace Fralle.Pingtap
{
	[CreateAssetMenu(menuName = "PlayerAttack/Effect/Slow")]
	public class SlowEffect : DamageEffect
	{
		[Header("Slow specific")]
		public float SlowModifier = 0.3f;

		public override void Enter(DamageController damageController)
		{
			Debug.LogWarning("SlowEffect: Code has been temporarily disabled. Please check code.");
			//if (health != null) health.GetComponent<EnemyNavigation>()?.AddModifier(name, slowModifier);
		}

		public override void Exit(DamageController damageController)
		{
			//if (health != null) health.GetComponent<EnemyNavigation>()?.RemoveModifier(name);
		}
	}
}
