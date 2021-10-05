using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Fralle.PingTap
{
  public class Weapon : MonoBehaviour
  {
    public event Action<Status> OnActiveWeaponActionChanged = delegate { };
    public event Action<int> OnAmmoChanged = delegate { };

    [FoldoutGroup("References")] public Transform[] Muzzles;
    [FoldoutGroup("References")] public Transform leftHandGrip;
    [FoldoutGroup("References")] public Transform rightHandGrip;
    [FoldoutGroup("References")] public Transform weaponCameraTransform;

    [FoldoutGroup("Timers and cooldowns")] public float EquipTime = 0.4f;
    [FoldoutGroup("Timers and cooldowns")] public float ReloadTime = 1f;

    public Status Status { get; private set; }

    [HideLabel]
    public AmmoAddon Ammo;

    [HideInInspector] public Animator Animator;
    [HideInInspector] public Combatant Combatant;
    [HideInInspector] public RecoilAddon RecoilAddon;
    [HideInInspector] public HeadbobAdjuster HeadbobAdjuster;
    [HideInInspector] public AttackAction PrimaryAttack;
    [HideInInspector] public AttackAction SecondaryAttack;
    [HideInInspector] public float AttackRange;
    [HideInInspector] public int WeaponSlotIndex;

    [Header("Debug")]
    [ReadOnly] public float NextAvailableShot;

    void Awake()
    {
      RecoilAddon = GetComponent<RecoilAddon>();
      HeadbobAdjuster = GetComponent<HeadbobAdjuster>();
      Animator = GetComponent<Animator>();
      Ammo.Setup(this);
      SetupAttackActions();
    }

    void Update()
    {
      if (Status == Status.Firing)
      {
        NextAvailableShot -= Time.deltaTime;
        if (NextAvailableShot <= 0)
          ChangeWeaponAction(Status.Ready);
      }
      else
        NextAvailableShot = 0;
    }

    void SetupAttackActions()
    {
      AttackAction[] attackActions = GetComponentsInChildren<AttackAction>();
      if (attackActions.Length > 2)
        Debug.LogWarning($"Weapon {name} has more attack actions than possible (2).");
      else if (attackActions.Length > 0)
      {
        PrimaryAttack = attackActions[0];
        SecondaryAttack = attackActions.Length == 2 ? attackActions[1] : null;

        AttackRange = Mathf.Max(Mathf.Min(PrimaryAttack.GetRange(), SecondaryAttack ? SecondaryAttack.GetRange() : 0f), 10f);
      }
    }

    public void ChangeWeaponAction(Status status)
    {
      Status = status;
      OnActiveWeaponActionChanged(status);
    }

    public void Reload()
    {
      if (Ammo.CurrentAmmo == Ammo.MaxAmmo || Status == Status.Reloading)
        return;

      ChangeWeaponAction(Status.Reloading);
      Animator.SetTrigger("Reload");
      Animator.speed = 1 / Ammo.ReloadTime;
    }

    public void Unequip()
    {
      ChangeWeaponAction(Status.Unequipping);
      Animator.SetTrigger("Unequip");
      Animator.speed = 1 / EquipTime;
    }

    public void Equip()
    {
      ChangeWeaponAction(Status.Equipping);
      gameObject.SetActive(true);
      Animator.SetTrigger("Equip");
      Animator.speed = 1 / EquipTime;
    }

    void OnReloadEnd()
    {
      Ammo.SetMaxAmmo();
      ChangeWeaponAction(Status.Ready);
    }

    void OnUnequipEnd()
    {
      ChangeWeaponAction(Status.Disabled);
      gameObject.SetActive(false);
    }

    void OnEquipEnd()
    {
      if (Status != Status.Equipping)
        return;

      HeadbobAdjuster?.Activate();
      RecoilAddon?.Activate();
      ChangeWeaponAction(Status.Ready);
    }
  }
}
