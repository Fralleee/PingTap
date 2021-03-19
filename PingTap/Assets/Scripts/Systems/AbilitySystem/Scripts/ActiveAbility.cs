using UnityEngine;

namespace Fralle.AbilitySystem
{
	public class ActiveAbility : Ability
	{
		public float cooldown = 0f;
		internal float cooldownTimer = 0f;

		public string ActivateButton = "Maybe use enum here";
		public bool IsReady => Time.time > cooldownTimer;

		public virtual void Update() { }

		public virtual void Perform()
		{
			cooldownTimer = Time.deltaTime + cooldown;
		}

		public override void Setup(AbilityController abilityController) { }
	}
}
