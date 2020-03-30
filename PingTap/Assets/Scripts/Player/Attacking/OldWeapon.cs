//using Fralle;
//using System.Collections;
//using UnityEngine;

//[RequireComponent(typeof(AmmoController))]
//[RequireComponent(typeof(RecoilController))]
//public abstract class OldWeapon : MonoBehaviour
//{
//  [Header("Information")]
//  [SerializeField] internal string weaponName;
//  [SerializeField] internal MouseButton fireInput = MouseButton.Left;

//  [Header("Equip")]
//  [SerializeField] internal float animationTime = 0.3f;

//  [Header("Shooting")]
//  [SerializeField] internal float kickbackForce = 0.15f;
//  [SerializeField] internal int shotsPerSecond = 20;
//  [SerializeField] internal bool tapable = false;
//  [SerializeField] internal float resetSmooth = 11.75f;
//  [SerializeField] internal Vector3 scopePos = new Vector3(-0.225f, 0.1f);
//  [SerializeField] internal Transform muzzle;

//  public bool performingAction;

//  internal float time;
//  internal bool scoping;
//  internal bool shooting;
//  internal Transform playerCamera;
//  internal Vector3 startPosition;
//  internal Quaternion startRotation;

//  internal AmmoController ammoController;
//  internal RecoilController recoilController;

//  internal virtual void Awake()
//  {
//    if (string.IsNullOrWhiteSpace(weaponName)) weaponName = name;
//    ammoController = GetComponent<AmmoController>();
//    recoilController = GetComponent<RecoilController>();
//  }

//  internal virtual void Update()
//  {
//    bool isEquipping = time < animationTime;
//    if (isEquipping)
//    {
//      time += Time.deltaTime;
//      time = Mathf.Clamp(time, 0f, animationTime);
//      var delta = -(Mathf.Cos(Mathf.PI * (time / animationTime)) - 1f) / 2f;
//      transform.localPosition = Vector3.Lerp(startPosition, Vector3.zero, delta);
//      transform.localRotation = Quaternion.Lerp(startRotation, Quaternion.identity, delta);
//    }
//    else
//    {
//      scoping = Input.GetMouseButton(1) && !ammoController.isReloading;
//      transform.localRotation = Quaternion.identity;
//      transform.localPosition = Vector3.Lerp(transform.localPosition, scoping ? scopePos : Vector3.zero, resetSmooth * Time.deltaTime);
//    }

//    bool shootWeapon = (tapable ? Input.GetMouseButtonDown((int)fireInput) : Input.GetMouseButton((int)fireInput)) && !shooting && !ammoController.isReloading;
//    if (shootWeapon)
//    {
//      ammoController.ChangeAmmo(-1);
//      Fire();
//      if (ammoController.HasAmmo()) StartCoroutine(ShootingCooldown());
//    }
//  }

//  internal IEnumerator ShootingCooldown()
//  {
//    shooting = true;
//    yield return new WaitForSeconds(1f / shotsPerSecond);
//    shooting = false;
//  }

//  public void Equip(Transform weaponHolder, Transform playerCamera)
//  {
//    time = 0f;
//    transform.parent = weaponHolder;

//    startPosition = transform.localPosition;
//    startRotation = transform.localRotation;

//    this.playerCamera = playerCamera;
//    recoilController.Initiate(playerCamera);
//  }

//  public abstract void Fire();

//  public bool Scoping => scoping;
//}
