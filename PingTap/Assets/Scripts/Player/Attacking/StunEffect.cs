using System.Collections;
using System.Collections.Generic;
using Fralle;
using UnityEngine;

public class StunEffect : DamageEffect
{
  public float chanceOnHit = 1f;
  public float damage = 1f;
  
  public override void Tick(DamageController damageController)
  {
    throw new System.NotImplementedException();
  }

  public override DamageEffect Recalculate(float modifier)
  {
    throw new System.NotImplementedException();
  }
}
