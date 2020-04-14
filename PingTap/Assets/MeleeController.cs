using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fralle;
using UnityEditor;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
  public float meleeDamage = 40f;
  public float meleeRadius = 2f;

  [SerializeField] KeyCode inputKey;

  [SerializeField] float swingTime = 0.25f;
  [SerializeField] float recoveryTime = 1f;

  bool swinging;
  float activeSwingTime;
  float activeRecoveryTime;
  Player player;
  WeaponManager weaponManager;
  Vector3 swingPosition = new Vector3(0.4f, 0, 0.75f);
  Quaternion swingRotation = Quaternion.Euler(0, -160f, 0);

  void Awake()
  {
    player = GetComponentInParent<Player>();
    weaponManager = GetComponent<WeaponManager>();
  }

  void Update()
  {
    if (weaponManager.equippedWeapon.activeWeaponAction != ActiveWeaponAction.READY) return;
    if (Input.GetKeyDown(inputKey) || Input.GetMouseButtonDown(3))
    {
      PerformMelee();
    }
  }

  void LateUpdate()
  {
    Animate();
  }

  void Animate()
  {
    if (activeSwingTime > 0)
    {
      activeSwingTime -= Time.deltaTime;
      if (swinging)
      {
        var delta = -(Mathf.Cos(Mathf.PI * (activeSwingTime / swingTime)) - 1f) / 2f;
        transform.localPosition = Vector3.Lerp(swingPosition, Vector3.zero, delta);
        transform.localRotation = Quaternion.Lerp(swingRotation, Quaternion.identity, delta);
      }
      else
      {
        var delta = -(Mathf.Cos(Mathf.PI * (activeSwingTime / swingTime)) - 1f) / 2f;
        transform.localPosition = Vector3.Lerp(Vector3.zero, swingPosition, delta);
        transform.localRotation = Quaternion.Lerp(Quaternion.identity, swingRotation, delta);
        if(activeSwingTime <= 0) weaponManager.equippedWeapon.activeWeaponAction = ActiveWeaponAction.READY;
      }
    }
    else if(activeRecoveryTime > 0)
    {
      activeRecoveryTime -= Time.deltaTime;
      swinging = false;
      if (activeRecoveryTime <= 0) activeSwingTime = swingTime;
    }
  }

  void PerformMelee()
  {
    activeSwingTime = swingTime;
    activeRecoveryTime = recoveryTime;
    swinging = true;
    weaponManager.equippedWeapon.activeWeaponAction = ActiveWeaponAction.MELEE;

    Collider[] targets = GetTargets();
    foreach (Collider col in targets)
    {
      if(!TargetInArc(col)) continue;

      var damageController = col.GetComponent<DamageController>();
      damageController.TakeDamage(new DamageData()
      {
        damage = meleeDamage,
        player = player
      });
    }

  }

  bool TargetInArc(Collider collider)
  {
    Vector3 vectorToCollider = (collider.transform.position - transform.position).normalized;
    return Vector3.Dot(vectorToCollider, transform.forward) > 0;
  }

  Collider[] GetTargets()
  {
    Collider[] colliders = Physics.OverlapSphere(transform.position, meleeRadius);
    return colliders.Where(x => x.GetComponent<DamageController>()).ToArray();
  }

}
