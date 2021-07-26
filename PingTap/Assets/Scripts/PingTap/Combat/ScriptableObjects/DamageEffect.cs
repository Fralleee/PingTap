using UnityEngine;

namespace Fralle.Pingtap
{
	public abstract class DamageEffect : ScriptableObject
	{
		[HideInInspector] public Combatant Attacker;

		[Header("General")] public int Level = 1;
		public Element Element;
		public float BaseDamageModifier = 1f;

		[Header("Stacking")] public int MaxStacks = 1;
		public int Stacks = 1;

		[Header("Timers")] public float Timer;
		public float Time = 3f;

		[HideInInspector] public float WeaponDamage;

		public virtual DamageEffect Setup(Combatant attacker, float weaponDamageAmount)
		{
			var instance = Instantiate(this);
			instance.Attacker = attacker;
			instance.WeaponDamage = weaponDamageAmount;
			instance.name = name;
			return instance;
		}

		public virtual DamageEffect Append(DamageEffect oldEffect = null)
		{
			if (oldEffect == null)
				return this;

			// Always run Append on the effect with highest level
			if (oldEffect.Level > Level)
				oldEffect.Append(this);

			if (oldEffect.Stacks < MaxStacks)
				Stacks += oldEffect.Stacks;
			else
				Stacks = oldEffect.Stacks;

			return this;
		}

		public virtual void Tick(DamageController damageController)
		{
			Timer += UnityEngine.Time.deltaTime;
		}

		public virtual void Enter(DamageController damageController)
		{
		}

		public virtual void Exit(DamageController damageController)
		{
		}

		public virtual DamageEffect Recalculate(float modifier) => this;
	}
}
