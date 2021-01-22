using CombatSystem.Combat.Damage;
using UnityEngine;

namespace CombatSystem.Effect
{
  [CreateAssetMenu(menuName = "PlayerAttack/Effect/DamageData over time")]
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

    public override void Tick(DamageController damageController)
    {
      base.Tick(damageController);
      lastDamageTimer += Time.deltaTime;

      if (!(lastDamageTimer > 1)) return;

      lastDamageTimer -= 1f;
      var damage = baseDamageModifier * DamageConverter.AsDamageModifier(damageModifier / time, damageApplication, damageController, weaponDamage);
      damageController.ReceiveAttack(new DamageData()
      {
        attacker = attacker,
        element = element,
        hitAngle = -1,
        position = damageController.transform.position,
        damageFromHit = false,
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