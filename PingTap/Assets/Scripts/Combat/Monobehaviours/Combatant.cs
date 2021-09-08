using Fralle.Core;
using Sirenix.OdinInspector;
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

    public CombatScoreData stats = new CombatScoreData();
    public CombatUpgrades modifiers = new CombatUpgrades();

    [Required] public Transform aimTransform;
    [Required] public Transform weaponHolder;
    [ReadOnly] public Weapon equippedWeapon;

    [Header("Settings")]
    [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
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
      if (!equippedWeapon || !primaryAction || primaryAction.tapable && !keyDown)
        return;

      primaryAction.Perform();
    }

    public void SecondaryAction(bool keyDown = false)
    {
      if (!equippedWeapon || !secondaryAction || secondaryAction.tapable && !keyDown)
        return;

      secondaryAction.Perform();
    }

    public void SuccessfulHit(DamageData damageData)
    {
      OnHit(damageData);
    }

    public void ClearWeapons()
    {
      string[] stringArray = { "Weapon Camera", "FPS" };
      foreach (Transform child in weaponHolder)
      {
        if (!stringArray.Any(child.name.Contains))
          DestroyImmediate(child.gameObject);
      }

      if (equippedWeapon)
        equippedWeapon = null;
    }

    public void EquipWeapon(Weapon weapon, bool animationDistance = true)
    {
      if (equippedWeapon != null && weapon != null && equippedWeapon.name == weapon.name)
        return;

      ClearWeapons();

      Weapon oldWeapon = equippedWeapon;
      Vector3 weaponHolderPosition = weaponHolder.position;
      Vector3 position = animationDistance ? weaponHolderPosition.With(y: -0.15f) : weaponHolderPosition;

      if (weapon != null)
      {
        equippedWeapon = Instantiate(weapon, position, weaponHolder.rotation, weaponHolder);
        equippedWeapon.Equip(this);

        SetupAttackActions();
      }
      else
      {
        equippedWeapon = null;
        primaryAction = null;
        secondaryAction = null;
      }

      if (equippedWeapon != null || oldWeapon != null)
        OnWeaponSwitch(equippedWeapon, oldWeapon);
    }

    void Awake()
    {
      teamController = GetComponent<TeamController>();

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
      AttackAction[] attackActions = equippedWeapon.GetComponentsInChildren<AttackAction>();
      if (attackActions.Length > 2)
        Debug.LogWarning($"Weapon {equippedWeapon} has more attack actions than possible (2).");
      else if (attackActions.Length > 0)
      {
        primaryAction = attackActions[0];
        secondaryAction = attackActions.Length == 2 ? attackActions[1] : null;

        AttackRange = Mathf.Max(Mathf.Min(primaryAction.GetRange(), secondaryAction ? secondaryAction.GetRange() : 0f), 10f);
      }
    }

    void OnDestroy()
    {
      if (ikHandler.enabled)
        ikHandler.Clean();
    }
  }
}
