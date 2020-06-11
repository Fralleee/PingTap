using Fralle.Attack.Addons;
using Fralle.Core.Attributes;
using Fralle.Core.Extensions;
using Fralle.Resource;
using System;
using UnityEngine;

namespace Fralle.Attack
{
  public class Weapon : InventoryItem
  {
    public event Action<Status> OnActiveWeaponActionChanged = delegate { };

    [Header("Weapon")]
    public string weaponName;
    [SerializeField] float equipAnimationTime = 0.3f;

    public Transform[] muzzles;
    public Status ActiveWeaponAction { get; private set; }

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

    public void Equip(Transform weaponHolder, Transform pCamera)
    {
      ActiveWeaponAction = Status.Equipping;
      equipTime = 0f;
      equipped = false;
      transform.parent = weaponHolder;

      var layer = LayerMask.NameToLayer("First Person Objects");
      gameObject.SetLayerRecursively(layer);

      startPosition = transform.localPosition;
      startRotation = transform.localRotation;

      playerCamera = pCamera;

      recoil.Initiate(pCamera);
    }

    public void ChangeWeaponAction(Status newActiveWeaponAction)
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