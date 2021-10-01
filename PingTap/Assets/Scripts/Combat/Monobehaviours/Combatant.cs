using Fralle.Core;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

namespace Fralle.PingTap
{
  public class Combatant : MonoBehaviour
  {
    public event Action<Weapon, Weapon> OnWeaponSwitch = delegate { };
    public event Action<DamageData> OnHit = delegate { };

    [HideInInspector] public TeamController teamController;

    [SerializeField] Weapon[] weaponSlots = new Weapon[3];

    [Required] public Transform aimTransform;
    [Required] public Transform weaponHolder;
    [ReadOnly] public Weapon equippedWeapon;

    [Header("Settings")]
    [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    public ImpactAtlas impactAtlas;

    public CombatScoreData stats = new CombatScoreData();
    public CombatUpgrades modifiers = new CombatUpgrades();

    [Header("Flags")]
    public bool hasActiveCamera;

    [Space(10)]
    [SerializeField] CombatIKHandler ikHandler;

    CombatTargetHandler targetHandler = new CombatTargetHandler();

    bool isEquipping;
    bool queuedWeaponSwitch;
    int queueWeaponIndex = -1;

    public float AttackRange { get; private set; } = 10f;

    void Awake()
    {
      teamController = GetComponent<TeamController>();

      if (ikHandler.enabled)
        ikHandler.Setup(this);
      targetHandler.Setup(this);
    }

    void Start()
    {
      SetupArsenal();
      EquipWeapon();
    }

    void FixedUpdate()
    {
      if (hasActiveCamera)
        targetHandler.DetectTargets();
    }

    void SetupArsenal()
    {
      for (int i = 0; i < weaponSlots.Length; i++)
      {
        var prefab = weaponSlots[i];
        if (prefab == null)
          continue;

        var instance = Instantiate(prefab, weaponHolder.position, weaponHolder.rotation, weaponHolder);
        instance.name = prefab.name;
        instance.WeaponSlotIndex = i;
        instance.Combatant = this;
        instance.gameObject.SetActive(false);
        weaponSlots[i] = instance;
      }
    }

    public void PrimaryAction(bool keyDown = false) => equippedWeapon?.PrimaryAttack?.Perform(keyDown);
    public void SecondaryAction(bool keyDown = false) => equippedWeapon?.SecondaryAttack?.Perform(keyDown);

    public void SuccessfulHit(DamageData damageData)
    {
      OnHit(damageData);
    }

    public void EquipWeapon(int index = 0)
    {
      if (weaponSlots[index] == null)
        return;

      if (!isEquipping)
        StartCoroutine(SwitchWeapon(index));
      else
      {
        queuedWeaponSwitch = true;
        queueWeaponIndex = index;
      }
    }

    IEnumerator SwitchWeapon(int index = 0)
    {
      isEquipping = true;

      Weapon oldWeapon = equippedWeapon;
      oldWeapon?.Unequip();
      if (oldWeapon)
        yield return new WaitForSeconds(oldWeapon.EquipTime);


      if (equippedWeapon?.WeaponSlotIndex != index)
        equippedWeapon = weaponSlots[index];
      else
        equippedWeapon = null;

      OnWeaponSwitch(equippedWeapon, oldWeapon);

      equippedWeapon?.Equip();
      if (equippedWeapon)
        yield return new WaitForSeconds(equippedWeapon.EquipTime);

      isEquipping = false;

      if (queuedWeaponSwitch)
      {
        StartCoroutine(SwitchWeapon(queueWeaponIndex));
        queueWeaponIndex = -1;
        queuedWeaponSwitch = false;
      }
    }

    void OnDestroy()
    {
      if (ikHandler.enabled)
        ikHandler.Clean();
    }
  }
}
