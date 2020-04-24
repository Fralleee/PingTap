using System.Collections;
using System.Collections.Generic;
using Fralle;
using UnityEngine;
using UnityScript.Steps;

[CreateAssetMenu(menuName = "Attack/Effect/Slow")]
public class SlowEffect : DamageEffect
{
  [Header("Slow specific")]
  public float slowModifier = 0.3f;
  
  public override void Enter(DamageController damageController)
  {
    damageController.GetComponent<AgentNavigation>().AddModifier(name, slowModifier);
  }

  public override void Exit(DamageController damageController)
  {
    damageController.GetComponent<AgentNavigation>().RemoveModifier(name);
  }

}
