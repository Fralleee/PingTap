using Fralle.Attack.Offense;
using UnityEngine;

namespace Fralle.Attack.Effect
{
  [CreateAssetMenu(menuName = "PlayerAttack/Effect/Damage over time")]
  public class DotEffect : DamageEffect
  {
    [Header("Dot specific")] public DamageType damageApplication = DamageType.Flat;
    public float damageModifier = 1f;
    [HideInInspector] public float lastDamageTimer;

    public override DamageEffect Append(DamageEffect oldEffect = null)
    {
      base.Append(oldEffect);
      if (oldEffect != null) lastDamageTimer = ((DotEffect)oldEffect).lastDamageTimer;
      return this;
    }

    public override void Tick(Health health)
    {
      base.Tick(health);
      lastDamageTimer += Time.deltaTime;

      if (!(lastDamageTimer > 1)) return;

      lastDamageTimer -= 1f;
      var damage = baseDamageModifier * DamageConverter.AsDamageModifier(damageModifier / time, damageApplication, health, weaponDamage);
      health.ReceiveAttack(new Damage()
      {
        player = player,
        element = element,
        hitAngle = -1,
        position = health.transform.position,
        damageAmount = damage * stacks
      });
    }

    public override DamageEffect Recalculate(float modifier)
    {
      damageModifier *= modifier;
      return this;
    }
  }
}