using Sirenix.OdinInspector;
using System;
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

    [FoldoutGroup("Animator Settings")] public float EquipTime;

    public Status ActiveWeaponAction { get; private set; }

    [HideInInspector] public Combatant Combatant;
    [HideInInspector] public RecoilAddon RecoilAddon;
    [HideInInspector] public AmmoAddon AmmoAddonController;
    [HideInInspector] public HeadbobAdjuster HeadbobAdjuster;
    [HideInInspector] public int WeaponSlotIndex;

    [Header("Debug")]
    [ReadOnly] public float NextAvailableShot;

    Animator Animator;

    void Awake()
    {
      RecoilAddon = GetComponent<RecoilAddon>();
      AmmoAddonController = GetComponent<AmmoAddon>();
      HeadbobAdjuster = GetComponent<HeadbobAdjuster>();
      Animator = GetComponent<Animator>();
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

      if (Input.GetKeyDown(KeyCode.R))
      {
        Reload();
      }
    }

    void Reload()
    {
      ActiveWeaponAction = Status.Reloading;

      Animator.SetTrigger("Reload");
      Animator.speed = 1 / AmmoAddonController.ReloadTime;
    }

    void OnReloadEnd()
    {
      //ChangeAmmo(MaxAmmo, false);
      ActiveWeaponAction = Status.Ready;
    }

    public void Unequip()
    {
      ActiveWeaponAction = Status.Equipping;

      Animator.SetTrigger("Unequip");
      Animator.speed = 1 / EquipTime;
    }

    void OnUnequipEnd()
    {
      ActiveWeaponAction = Status.NotEquipped;
      gameObject.SetActive(false);
    }


    public void Equip()
    {
      ActiveWeaponAction = Status.Equipping;
      gameObject.SetActive(true);

      Animator.SetTrigger("Equip");
      Animator.speed = 1 / EquipTime;
    }

    void OnEquipEnd()
    {
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
