﻿using CombatSystem.Addons;
using CombatSystem.Combat;
using CombatSystem.Enums;
using CombatSystem.Interfaces;
using System;
using UnityEngine;

namespace CombatSystem
{
  public class Weapon : MonoBehaviour, IWeapon
  {
    public event Action<Status> OnActiveWeaponActionChanged = delegate { };

    [Header("Weapon")]
    public string weaponName;
    [SerializeField] float equipAnimationTime = 0.3f;
    public Transform[] muzzles;

    public Status ActiveWeaponAction { get; private set; }

    [HideInInspector] public Combatant combatant;
    [HideInInspector] public RecoilAddon recoilAddon;
    [HideInInspector] public AmmoAddon ammoAddonController;

    public bool isEquipped { get; private set; }

    float equipTime;
    bool equipped;
    Vector3 startPosition;
    Quaternion startRotation;

    void Awake()
    {
      if (string.IsNullOrWhiteSpace(weaponName)) weaponName = name;

      recoilAddon = GetComponent<RecoilAddon>();
      ammoAddonController = GetComponent<AmmoAddon>();
    }

    void Update()
    {
      PerformEquip();
    }

    public void Equip(Combatant c)
    {
      ActiveWeaponAction = Status.Equipping;
      equipTime = 0f;
      equipped = false;
      combatant = c;

      startPosition = transform.localPosition;
      startRotation = transform.localRotation;

      isEquipped = true;
    }

    public void ChangeWeaponAction(Status newActiveWeaponAction)
    {
      ActiveWeaponAction = newActiveWeaponAction;
      OnActiveWeaponActionChanged(newActiveWeaponAction);
    }

    void PerformEquip()
    {
      if (equipped) return;

      var isEquipping = equipTime < equipAnimationTime;
      if (!isEquipping)
      {
        equipped = true;
        ActiveWeaponAction = Status.Ready;
        return;
      }

      equipTime += Time.deltaTime;
      equipTime = Mathf.Clamp(equipTime, 0f, equipAnimationTime);
      var delta = -(Mathf.Cos(Mathf.PI * (equipTime / equipAnimationTime)) - 1f) / 2f;
      transform.localPosition = Vector3.Lerp(startPosition, Vector3.zero, delta);
      transform.localRotation = Quaternion.Lerp(startRotation, Quaternion.identity, delta);
    }
  }
}