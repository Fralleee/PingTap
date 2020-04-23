using System.Collections;
using System.Collections.Generic;
using Fralle;
using UnityEngine;

public abstract class DamageEffect : ScriptableObject
{
  [HideInInspector] public Player player;

  [Header("General")]
  public int level = 1;
  public Element element;

  [Header("Stacking")]
  public int maxStacks = 1;
  public int stacks = 1;

  [Header("Timers")]
  public float timer;
  public float time = 3f;

  [HideInInspector] public float weaponDamage;
  [HideInInspector] public bool validEffect = true;

  public virtual DamageEffect Setup(DamageData damageData)
  {
    DamageEffect instance = Instantiate(this);
    instance.player = damageData.player;
    instance.weaponDamage = damageData.damage;
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

  public abstract void Tick(DamageController damageController);
  public abstract DamageEffect Recalculate(float modifier);
}
