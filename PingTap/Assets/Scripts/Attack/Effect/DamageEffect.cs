using Fralle.Attack.Offense;
using Fralle.Player;
using UnityEngine;

namespace Fralle.Attack.Effect
{
  public abstract class DamageEffect : ScriptableObject
  {
    [HideInInspector] public PlayerMain player;

    [Header("General")] public int level = 1;
    public Element element;
    public float baseDamageModifier = 1f;

    [Header("Stacking")] public int maxStacks = 1;
    public int stacks = 1;

    [Header("Timers")] public float timer;
    public float time = 3f;

    [HideInInspector] public float weaponDamage;

    public virtual DamageEffect Setup(PlayerMain pl, float weaponDamageAmount)
    {
      var instance = Instantiate(this);
      instance.player = pl;
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

    public virtual void Tick(Health health)
    {
      timer += Time.deltaTime;
    }

    public virtual void Enter(Health health)
    {
    }

    public virtual void Exit(Health health)
    {
    }

    public virtual DamageEffect Recalculate(float modifier) => this;
  }
}