using System.Collections;
using System.Collections.Generic;
using Fralle;
using UnityEngine;

public class SlowEffect : DamageEffect
{
  public float ticRate = 1f;
  public float totalDamage = 1f;
  
  public override void Tick(DamageController damageController)
  {
    throw new System.NotImplementedException();
  }

  public override DamageEffect Recalculate(float modifier)
  {
    throw new System.NotImplementedException();
  }
}
