using System.Collections;
using System.Collections.Generic;
using Fralle;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
  public BodyPartType bodyPartType;

  DamageController damageController;

  void Awake()
  {
    damageController = GetComponentInParent<DamageController>();
    if(damageController == null) Debug.LogError($"{gameObject.name}s BodyPart is missing DamageController in parent");
  }
  
  public void ApplyHit(DamageData damageData)
  {
    damageController.Hit(damageData);
  }

}
