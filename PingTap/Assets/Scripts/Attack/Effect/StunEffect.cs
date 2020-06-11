using Fralle.AI;
using Fralle.Attack.Offense;
using UnityEngine;

namespace Fralle.Attack.Effect
{
  [CreateAssetMenu(menuName = "PlayerAttack/Effect/Stun")]
  public class StunEffect : DamageEffect
  {
    public float chanceOnHit = 0.25f;
    public float damage = 10f;

    public override void Enter(Health health)
    {
      var randValue = Random.value;
      if (randValue <= chanceOnHit)
      {
        health.ReceiveAttack(new Damage()
        {
          element = element,
          hitAngle = -1,
          player = player,
          position = health.transform.position,
          damageAmount = damage
        });
      }
      health.GetComponent<EnemyNavigation>().AddModifier(name, 0f);
    }

    public override void Exit(Health health)
    {
      health.GetComponent<EnemyNavigation>().RemoveModifier(name);
    }
  }
}