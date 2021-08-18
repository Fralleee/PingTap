using Fralle.Core;
using System;
using System.Linq;
using UnityEngine;

namespace Fralle.PingTap
{
  public class Combatant : MonoBehaviour
  {
    public event Action<Weapon, Weapon> OnWeaponSwitch = delegate { };
    public event Action<DamageData> OnHit = delegate { };

    [HideInInspector] public TeamController teamController;

    public CombatScoreData Stats = new CombatScoreData();
    public CombatUpgrades Modifiers = new CombatUpgrades();

    public Transform AimTransform;
    public Transform WeaponHolder;
    [Readonly] public Weapon EquippedWeapon;

    [Header("Settings")]
    [Expandable]
    public ImpactAtlas impactAtlas;

    [Header("Flags")]
    public bool hasActiveCamera;

    [Space(10)]
    [SerializeField] CombatIKHandler ikHandler;

    CombatTargetHandler targetHandler = new CombatTargetHandler();
    AttackAction primaryAction;
    AttackAction secondaryAction;

    public float AttackRange { get; private set; } = 10f;

    public void PrimaryAction(bool keyDown = false)
    {
      if (!EquippedWeapon || !primaryAction || primaryAction.Tapable && !keyDown)
        return;

      primaryAction.Perform();
    }

    public void SecondaryAction(bool keyDown = false)
    {
      if (!EquippedWeapon || !secondaryAction || secondaryAction.Tapable && !keyDown)
        return;

      secondaryAction.Perform();
    }

    public void SetFpsLayers(string layerName)
    {
      int layer = LayerMask.NameToLayer(layerName);
      EquippedWeapon.gameObject.SetLayerRecursively(layer);
    }

    public void SuccessfulHit(DamageData damageData)
    {
      OnHit(damageData);
    }

    public void ClearWeapons()
    {
      string[] stringArray = { "Weapon Camera", "FPS" };
      foreach (Transform child in WeaponHolder)
      {
        if (!stringArray.Any(child.name.Contains))
          DestroyImmediate(child.gameObject);
      }

      if (EquippedWeapon)
        EquippedWeapon = null;
    }

    public void EquipWeapon(Weapon weapon, bool animationDistance = true)
    {
      if (EquippedWeapon != null && weapon != null && EquippedWeapon.name == weapon.name)
        return;

      ClearWeapons();

      Weapon oldWeapon = EquippedWeapon;
      Vector3 position = animationDistance ? WeaponHolder.position.With(y: -0.15f) : WeaponHolder.position;

      if (weapon != null)
      {
        EquippedWeapon = Instantiate(weapon, position, WeaponHolder.rotation, WeaponHolder);
        EquippedWeapon.Equip(this);

        SetupAttackActions();
      }
      else
      {
        EquippedWeapon = null;
        primaryAction = null;
        secondaryAction = null;
      }

      if (EquippedWeapon != null || oldWeapon != null)
        OnWeaponSwitch(EquippedWeapon, oldWeapon);
    }

    void Awake()
    {
      SetDefaults();

      if (ikHandler.enabled)
        ikHandler.Setup(this);
      targetHandler.Setup(this);
    }

    void FixedUpdate()
    {
      if (hasActiveCamera)
        targetHandler.DetectTargets();
    }

    void SetupAttackActions()
    {
      AttackAction[] attackActions = EquippedWeapon.GetComponentsInChildren<AttackAction>();
      if (attackActions.Length > 2)
        Debug.LogWarning($"Weapon {EquippedWeapon} has more attack actions than possible (2).");
      else if (attackActions.Length > 0)
      {
        primaryAction = attackActions[0];
        secondaryAction = attackActions.Length == 2 ? attackActions[1] : null;

        AttackRange = Mathf.Max(Mathf.Min(primaryAction.GetRange(), secondaryAction ? secondaryAction.GetRange() : 0f), 10f);
      }
    }

    void SetDefaults()
    {
      if (AimTransform == null)
        AimTransform = transform;
      if (WeaponHolder == null)
        WeaponHolder = transform;

      teamController = GetComponent<TeamController>();
    }

    void OnDestroy()
    {
      if (ikHandler.enabled)
        ikHandler.Clean();
    }
  }
}
