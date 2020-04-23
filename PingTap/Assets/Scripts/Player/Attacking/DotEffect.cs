using System.Collections;
using System.Collections.Generic;
using Fralle;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack/DotEffect")]
public class DotEffect : DamageEffect
{
  [Header("Dot specific")]
  public DamageApplication damageApplication = DamageApplication.Flat;
  public float damageModifier = 1f;
  [HideInInspector] public float lastDamageTimer;

  public override DamageEffect Append(DamageEffect oldEffect = null)
  {
    base.Append(oldEffect);
    if (oldEffect != null) lastDamageTimer = ((DotEffect) oldEffect).lastDamageTimer;
    return this;
  }

  public override void Tick(DamageController damageController)
  {
    timer += Time.deltaTime;
    lastDamageTimer += Time.deltaTime;

    if (!(lastDamageTimer > 1)) return;

    lastDamageTimer -= 1f;
    float damage = DamageConverter.AsDamageModifier(damageModifier / time, damageApplication, damageController, weaponDamage); ;
    damageController.TakeDamage(new DamageData()
    {
      player = player,
      position = damageController.transform.position,
      damage = damage * stacks
    });
  }

  public override DamageEffect Recalculate(float modifier)
  {
    damageModifier *= modifier;
    return this;
  }
}
