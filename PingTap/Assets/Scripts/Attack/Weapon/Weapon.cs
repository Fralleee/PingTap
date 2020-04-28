using System;
using UnityEngine;

namespace Fralle.Attack
{
  public class Weapon : InventoryItem
  {
    public event Action<WeaponStatus> OnActiveWeaponActionChanged = delegate { };

    [Header("Weapon")]
    public string weaponName;
    [SerializeField] float equipAnimationTime = 0.3f;

    public Transform[] muzzles;
    public WeaponStatus ActiveWeaponAction { get; private set; }

    [HideInInspector] public Transform playerCamera;
    [HideInInspector] public Recoil recoil;
    [HideInInspector] public Ammo ammoController;

    [Readonly] public float weaponScore;

    float equipTime;
    bool equipped;
    Vector3 startPosition;
    Quaternion startRotation;

    void Awake()
    {
      if (string.IsNullOrWhiteSpace(weaponName)) weaponName = name;

      recoil = GetComponent<Recoil>();
      ammoController = GetComponent<Ammo>();
    }

    void Start()
    {
      CalculateWeaponScore();
    }

    void Update()
    {
      PerformEquip();
    }

    public void Equip(Transform weaponHolder, Transform playerCamera)
    {
      ActiveWeaponAction = WeaponStatus.Equipping;
      equipTime = 0f;
      equipped = false;
      transform.parent = weaponHolder;

      int layer = LayerMask.NameToLayer("First Person Objects");
      gameObject.SetLayerRecursively(layer);

      startPosition = transform.localPosition;
      startRotation = transform.localRotation;

      this.playerCamera = playerCamera;

      recoil.Initiate(playerCamera);
    }

    public void ChangeWeaponAction(WeaponStatus newActiveWeaponAction)
    {
      ActiveWeaponAction = newActiveWeaponAction;
      OnActiveWeaponActionChanged(newActiveWeaponAction);
    }

    void CalculateWeaponScore()
    {
      weaponScore = 1f;
    }

    void PerformEquip()
    {
      if (equipped) return;

      bool isEquipping = equipTime < equipAnimationTime;
      if (!isEquipping)
      {
        equipped = true;
        ActiveWeaponAction = WeaponStatus.Ready;
        return;
      }

      equipTime += Time.deltaTime;
      equipTime = Mathf.Clamp(equipTime, 0f, equipAnimationTime);
      float delta = -(Mathf.Cos(Mathf.PI * (equipTime / equipAnimationTime)) - 1f) / 2f;
      transform.localPosition = Vector3.Lerp(startPosition, Vector3.zero, delta);
      transform.localRotation = Quaternion.Lerp(startRotation, Quaternion.identity, delta);
    }
  }
}