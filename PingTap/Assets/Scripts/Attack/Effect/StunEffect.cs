﻿using Fralle.Attack.Offense;
using Fralle.Movement;
using UnityEngine;

namespace Fralle.Attack.Effect
{
  [CreateAssetMenu(menuName = "Attack/Effect/Stun")]
  public class StunEffect : DamageEffect
  {
    public float chanceOnHit = 0.25f;
    public float damage = 10f;

    public override void Enter(Health health)
    {
      float randValue = Random.value;
      if (randValue <= chanceOnHit)
      {
        health.ReceiveAttack(new Damage()
        {
          element = element,
          hitAngle = -1,
          hitBoxType = HitBoxType.Major,
          player = player,
          position = health.transform.position,
          damageAmount = damage
        });
      }
      health.GetComponent<AgentNavigation>().AddModifier(name, 0f);
    }

    public override void Exit(Health health)
    {
      health.GetComponent<AgentNavigation>().RemoveModifier(name);
    }
  }
}