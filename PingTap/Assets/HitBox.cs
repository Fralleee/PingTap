using System.Collections;
using System.Collections.Generic;
using Fralle;
using UnityEngine;

public class HitBox : MonoBehaviour
{
  public HitBoxType hitBoxType;

  DamageController damageController;

  void Awake()
  {
    damageController = GetComponentInParent<DamageController>();
    if(damageController == null) Debug.LogError($"{gameObject.name}s hitbox is missing DamageController in parent");
  }
  
  public void ApplyHit(DamageData damageData)
  {
    damageController.Hit(damageData);
  }

}
