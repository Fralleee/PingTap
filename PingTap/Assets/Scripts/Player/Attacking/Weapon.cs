using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Fralle
{
  public class Weapon : InventoryItem
  {
    public event Action<ActiveWeaponAction> OnActiveWeaponActionChanged = delegate { };

    [Header("Weapon")]
    public string weaponName;
    [SerializeField] float equipAnimationTime = 0.3f;

    public GameObject graphics;
    public Transform[] muzzles;
    public ActiveWeaponAction ActiveWeaponAction { get; private set; }

    [HideInInspector] public Transform playerCamera;
    [HideInInspector] public RecoilController recoilController;
    [HideInInspector] public AmmoController ammoController;

    [Readonly] public float weaponScore;

    float equipTime;
    Vector3 startPosition;
    Quaternion startRotation;

    void Awake()
    {
      if (string.IsNullOrWhiteSpace(weaponName)) weaponName = name;

      recoilController = GetComponent<RecoilController>();
      ammoController = GetComponent<AmmoController>();

      if (graphics == null) graphics = transform.Find("Graphics").gameObject;
    }

    void Start()
    {
      CalculateWeaponScore();
    }

    void Update()
    {
      PerformEquip();
    }

    void CalculateWeaponScore()
    {
      weaponScore = 1f;
    }
     
    void PerformEquip()
    {
      bool isEquipping = equipTime < equipAnimationTime;
      if (!isEquipping) return;

      equipTime += Time.deltaTime;
      equipTime = Mathf.Clamp(equipTime, 0f, equipAnimationTime);
      float delta = -(Mathf.Cos(Mathf.PI * (equipTime / equipAnimationTime)) - 1f) / 2f;
      transform.localPosition = Vector3.Lerp(startPosition, Vector3.zero, delta);
      transform.localRotation = Quaternion.Lerp(startRotation, Quaternion.identity, delta);
    }

    public void Equip(Transform weaponHolder, Transform playerCamera)
    {
      equipTime = 0f;
      transform.parent = weaponHolder;

      int layer = LayerMask.NameToLayer("First Person Objects");
      gameObject.SetLayerRecursively(layer);

      startPosition = transform.localPosition;
      startRotation = transform.localRotation;

      this.playerCamera = playerCamera;

      recoilController.Initiate(playerCamera);
    }

    public void ChangeWeaponAction(ActiveWeaponAction newActiveWeaponAction)
    {
      ActiveWeaponAction = newActiveWeaponAction;
      OnActiveWeaponActionChanged(newActiveWeaponAction);
    }
  }
}