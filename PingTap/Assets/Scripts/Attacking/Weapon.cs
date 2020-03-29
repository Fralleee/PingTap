using UnityEngine;

namespace Fralle
{
  public class Weapon : MonoBehaviour
  {
    [Header("Weapon")]
    public string weaponName;
    [SerializeField] float equipAnimationTime = 0.3f;

    public Transform muzzle;
    public bool performingAction;

    [HideInInspector] public Transform playerCamera;
    [HideInInspector] public RecoilController recoilController;
    [HideInInspector] public AmmoController ammoController;

    float time;
    Vector3 startPosition;
    Quaternion startRotation;


    internal virtual void Awake()
    {
      if (string.IsNullOrWhiteSpace(weaponName)) weaponName = name;
      recoilController = GetComponent<RecoilController>();
      ammoController = GetComponent<AmmoController>();
    }

    internal virtual void Update()
    {
      bool isEquipping = time < equipAnimationTime;
      if (!isEquipping) return;

      time += Time.deltaTime;
      time = Mathf.Clamp(time, 0f, equipAnimationTime);
      var delta = -(Mathf.Cos(Mathf.PI * (time / equipAnimationTime)) - 1f) / 2f;
      transform.localPosition = Vector3.Lerp(startPosition, Vector3.zero, delta);
      transform.localRotation = Quaternion.Lerp(startRotation, Quaternion.identity, delta);
    }

    public void Equip(Transform weaponHolder, Transform playerCamera)
    {
      time = 0f;
      transform.parent = weaponHolder;

      startPosition = transform.localPosition;
      startRotation = transform.localRotation;

      this.playerCamera = playerCamera;
      recoilController.Initiate(playerCamera);
    }
  }
}