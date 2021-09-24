using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

namespace Fralle.PingTap
{
  public class Weapon : MonoBehaviour
  {
    public event Action<Status> OnActiveWeaponActionChanged = delegate { };

    [Header("Transforms")]
    public Transform[] Muzzles;
    public Transform leftHandGrip;
    public Transform rightHandGrip;
    public Transform weaponCameraTransform;

    public Status ActiveWeaponAction { get; private set; }

    [HideInInspector] public Combatant Combatant;
    [HideInInspector] public RecoilAddon RecoilAddon;
    [HideInInspector] public AmmoAddon AmmoAddonController;
    [HideInInspector] public HeadbobAdjuster HeadbobAdjuster;
    [HideInInspector] public int WeaponSlotIndex;

    [Header("Debug")]
    [ReadOnly] public float NextAvailableShot;

    void Awake()
    {
      RecoilAddon = GetComponent<RecoilAddon>();
      AmmoAddonController = GetComponent<AmmoAddon>();
      HeadbobAdjuster = GetComponent<HeadbobAdjuster>();
    }

    void Update()
    {
      if (ActiveWeaponAction == Status.Firing)
      {
        NextAvailableShot -= Time.deltaTime;
        if (NextAvailableShot <= 0)
          ChangeWeaponAction(Status.Ready);
      }
      else
      {
        NextAvailableShot = 0;
      }
    }

    public IEnumerator Unequip(float equipTime)
    {
      ActiveWeaponAction = Status.Equipping;

      Combatant.weaponAnimator.AnimateUnequip(Combatant, equipTime);
      yield return new WaitForSeconds(equipTime);

      ActiveWeaponAction = Status.NotEquipped;
      gameObject.SetActive(false);
    }

    public IEnumerator Equip(float equipTime)
    {
      gameObject.SetActive(true);
      ActiveWeaponAction = Status.Equipping;

      Combatant.weaponAnimator.AnimateEquip(Combatant, equipTime);
      yield return new WaitForSeconds(equipTime);

      HeadbobAdjuster?.Activate();
      RecoilAddon?.Activate();
      ActiveWeaponAction = Status.Ready;
    }

    public void ChangeWeaponAction(Status newActiveWeaponAction)
    {
      ActiveWeaponAction = newActiveWeaponAction;
      OnActiveWeaponActionChanged(newActiveWeaponAction);
    }
  }
}
