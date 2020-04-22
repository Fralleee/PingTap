using System.Collections;
using System.Collections.Generic;
using Fralle;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack/DamageType")]
public class DamageType : ScriptableObject
{
  [SerializeField] DamageEffect[] damageEffects;
  public void ApplyEffect(DamageController damageController, DamageData damageData)
  {
    foreach (DamageEffect damageEffect in damageEffects)
    {
      DamageEffect instance = Instantiate(damageEffect);
      instance.player = damageData.player;
      instance.weaponDamage = damageData.damage;
      damageController.ApplyEffect(instance);
    }
      
  }

}
