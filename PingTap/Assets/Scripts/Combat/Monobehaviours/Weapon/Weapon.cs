using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Fralle.PingTap
{
  public class Weapon : MonoBehaviour
  {
    public event Action<Status> OnActiveWeaponActionChanged = delegate { };

    [Header("Weapon")]
    public string WeaponName;
    [SerializeField] float equipAnimationTime = 0.3f;

    [Header("Transforms")]
    public Transform[] Muzzles;
    public Transform leftHandGrip;
    public Transform rightHandGrip;
    public Transform weaponCameraTransform;

    public Status ActiveWeaponAction { get; private set; }

    [HideInInspector] public Combatant Combatant;
    [HideInInspector] public RecoilAddon RecoilAddon;
    [HideInInspector] public AmmoAddon AmmoAddonController;

    public bool IsEquipped { get; private set; }

    [Header("Debug")]
    [ReadOnly] public float NextAvailableShot;

    float equipTime;
    bool animationComplete;
    Vector3 startPosition;
    Quaternion startRotation;

    void Update()
    {
      if (!animationComplete)
        AnimateEquip();

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
    }

    public void Equip(Combatant combatant, bool shouldAnimate = true)
    {
      if (string.IsNullOrWhiteSpace(WeaponName))
        WeaponName = name;

      ActiveWeaponAction = Status.Equipping;
      equipTime = 0f;
      animationComplete = !shouldAnimate;
      Combatant = combatant;

      startPosition = shouldAnimate ? transform.localPosition : Vector3.zero;
      startRotation = shouldAnimate ? transform.localRotation : Quaternion.identity;

      RecoilAddon = GetComponent<RecoilAddon>();
      RecoilAddon.Activate();
      AmmoAddonController = GetComponent<AmmoAddon>();

      IsEquipped = true;
    }

    public void ChangeWeaponAction(Status newActiveWeaponAction)
    {
      ActiveWeaponAction = newActiveWeaponAction;
      OnActiveWeaponActionChanged(newActiveWeaponAction);
    }

    void AnimateEquip()
    {
      equipTime += Time.deltaTime;
      equipTime = Mathf.Clamp(equipTime, 0f, equipAnimationTime);
      float delta = -(Mathf.Cos(Mathf.PI * (equipTime / equipAnimationTime)) - 1f) / 2f;
      transform.localPosition = Vector3.Lerp(startPosition, Vector3.zero, delta);
      transform.localRotation = Quaternion.Lerp(startRotation, Quaternion.identity, delta);

      if (equipTime >= equipAnimationTime)
      {
        animationComplete = true;
        ActiveWeaponAction = Status.Ready;
        return;
      }
    }
  }
}
