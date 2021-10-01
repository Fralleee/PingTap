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

    public Status ActiveWeaponAction { get; private set; }

    [HideLabel]
    public AmmoAddon Ammo;

    [HideInInspector] public Combatant Combatant;
    [HideInInspector] public RecoilAddon RecoilAddon;

    [HideInInspector] public HeadbobAdjuster HeadbobAdjuster;
    [HideInInspector] public AttackAction PrimaryAttack;
    [HideInInspector] public AttackAction SecondaryAttack;
    [HideInInspector] public float AttackRange;
    [HideInInspector] public int WeaponSlotIndex;

    [Header("Debug")]
    [ReadOnly] public float NextAvailableShot;

    Animator Animator;

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

    public void ChangeWeaponAction(Status newActiveWeaponAction)
    {
      ActiveWeaponAction = newActiveWeaponAction;
      OnActiveWeaponActionChanged(newActiveWeaponAction);
    }

    public void Reload()
    {
      ActiveWeaponAction = Status.Reloading;

      Animator.SetTrigger("Reload");
      Animator.speed = 1 / Ammo.ReloadTime;
    }

    public void Unequip()
    {
      ActiveWeaponAction = Status.Equipping;

      Animator.SetTrigger("Unequip");
      Animator.speed = 1 / EquipTime;
    }

    public void Equip()
    {
      ActiveWeaponAction = Status.Equipping;
      gameObject.SetActive(true);

      Animator.SetTrigger("Equip");
      Animator.speed = 1 / EquipTime;
    }

    void OnReloadEnd()
    {
      Ammo.SetMaxAmmo();
      ActiveWeaponAction = Status.Ready;
    }

    void OnUnequipEnd()
    {
      ActiveWeaponAction = Status.NotEquipped;
      gameObject.SetActive(false);
    }

    void OnEquipEnd()
    {
      HeadbobAdjuster?.Activate();
      RecoilAddon?.Activate();
      ActiveWeaponAction = Status.Ready;
    }
  }
}
