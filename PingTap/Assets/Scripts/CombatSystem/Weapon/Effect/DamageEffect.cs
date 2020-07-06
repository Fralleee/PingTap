using CombatSystem.Combat;
using CombatSystem.Combat.Damage;
using CombatSystem.Enums;
using UnityEngine;

namespace CombatSystem.Effect
{
  public abstract class DamageEffect : ScriptableObject
  {
    [HideInInspector] public Combatant attacker;

    [Header("General")] public int level = 1;
    public Element element;
    public float baseDamageModifier = 1f;

    [Header("Stacking")] public int maxStacks = 1;
    public int stacks = 1;

    [Header("Timers")] public float timer;
    public float time = 3f;

    [HideInInspector] public float weaponDamage;

    public virtual DamageEffect Setup(Combatant attacker, float weaponDamageAmount)
    {
      var instance = Instantiate(this);
      instance.attacker = attacker;
      instance.weaponDamage = weaponDamageAmount;
      instance.name = name;
      return instance;
    }

    public virtual DamageEffect Append(DamageEffect oldEffect = null)
    {
      if (oldEffect == null) return this;

      // Always run Append on the effect with highest level
      if (oldEffect.level > level) oldEffect.Append(this);

      if (oldEffect.stacks < maxStacks) stacks += oldEffect.stacks;
      else stacks = oldEffect.stacks;

      return this;
    }

    public virtual void Tick(DamageController damageController)
    {
      timer += Time.deltaTime;
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