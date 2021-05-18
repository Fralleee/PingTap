using CombatSystem.Combat.Damage;
using UnityEngine;

namespace CombatSystem.Effect
{
  [CreateAssetMenu(menuName = "PlayerAttack/Effect/Stun")]
  public class StunEffect : DamageEffect
  {
    public float ChanceOnHit = 0.25f;
    public float Damage = 10f;

    public override void Enter(DamageController damageController)
    {
      Debug.LogWarning("StunEffect: Code has been temporarily disabled. Please check code.");
      //var randValue = Random.value;
      //if (randValue <= chanceOnHit)
      //{
      //  health.ReceiveAttack(new DamageData()
      //  {
      //    element = element,
      //    hitAngle = -1,
      //    player = player,
      //    position = health.transform.position,
      //    damageAmount = damage
      //  });
      //}
      //health.GetComponent<EnemyNavigation>().AddModifier(name, 0f);
    }

    public override void Exit(DamageController damageController)
    {
      //health.GetComponent<EnemyNavigation>().RemoveModifier(name);
    }
  }
}