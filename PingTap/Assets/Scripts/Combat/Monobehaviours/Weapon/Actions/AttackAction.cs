using UnityEngine;

namespace Fralle.PingTap
{
  [RequireComponent(typeof(Weapon))]
  [RequireComponent(typeof(AmmoAddon))]
  [RequireComponent(typeof(RecoilAddon))]
  public abstract class AttackAction : MonoBehaviour
  {
    [Header("Shooting")]
    [SerializeField] internal float minDamage = 1;
    [SerializeField] internal float maxDamage = 10;
    [SerializeField] internal int ammoPerShot = 1;
    [SerializeField] internal int shotsPerSecond = 20;
    [SerializeField] internal bool tapable = false;
    [SerializeField] internal Element element;
    [SerializeField] internal DamageEffect[] damageEffects = new DamageEffect[0];

    internal Weapon Weapon;
    internal Combatant Combatant;
    int nextMuzzle;

    float fireRate;

    internal float Damage => Random.Range(minDamage, maxDamage);
    bool HasAmmo => Weapon.AmmoAddonController && Weapon.AmmoAddonController.HasAmmo();

    internal virtual void Awake()
    {
      OnValidate();
    }

    internal virtual void Start()
    {
      Weapon = GetComponent<Weapon>();
      Combatant = Weapon.Combatant;
    }

    internal virtual void OnValidate()
    {
      fireRate = 1f / shotsPerSecond;
    }

    public void Perform()
    {
      if (!Weapon || Weapon.ActiveWeaponAction != Status.Ready)
        return;

      int shotsToFire = Mathf.RoundToInt(-Weapon.NextAvailableShot / fireRate);
      for (int i = 0; i <= shotsToFire; i++)
      {
        Fire();
        Weapon.NextAvailableShot += fireRate;
        Weapon.AmmoAddonController.ChangeAmmo(-ammoPerShot);

        if (Weapon.RecoilAddon)
          Weapon.RecoilAddon.AddRecoil();

        if (!HasAmmo)
          break;
      }

      if (HasAmmo)
      {
        Weapon.ChangeWeaponAction(Status.Firing);
      }
    }

    public abstract void Fire();
    public abstract float GetRange();

    internal Transform GetMuzzle()
    {
      Transform muzzle = Weapon.Muzzles[nextMuzzle];
      if (Weapon.Muzzles.Length <= 1)
        return muzzle;

      nextMuzzle++;
      if (nextMuzzle > Weapon.Muzzles.Length - 1)
        nextMuzzle = 0;

      return muzzle;
    }
  }
}
