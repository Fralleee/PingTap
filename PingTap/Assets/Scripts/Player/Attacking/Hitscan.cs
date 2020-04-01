using Fralle;
using UnityEngine;

public class Hitscan : WeaponAction
{
  [Header("Hitscan")]
  [SerializeField] float range = 50;
  [SerializeField] float pushForce = 3.5f;
  [SerializeField] GameObject impactParticlePrefab;
  [SerializeField] GameObject muzzleParticlePrefab;

  [Header("Spread")]
  [SerializeField] bool useSpread;
  [SerializeField] float spreadIncreaseEachShot = 0.75f;
  [SerializeField] float maxSpread = 5f;
  [SerializeField] float recovery = 5f;

  [Readonly] public float currentSpread;

  override internal void Update()
  {
    base.Update();
    if (currentSpread != 0)
    {
      currentSpread -= Time.deltaTime * recovery;
      currentSpread = Mathf.Clamp(currentSpread, 0, maxSpread);
    }
  }

  public override void Fire()
  {
    Vector3 forward = weapon.playerCamera.forward;

    if (useSpread) forward = CalculateBulletSpread();

    if (Physics.Raycast(weapon.playerCamera.position, forward, out var hitInfo, range))
    {
      var rb = hitInfo.transform.GetComponent<Rigidbody>();
      if (rb != null) rb.AddForce(weapon.playerCamera.forward * pushForce);

      if (muzzleParticlePrefab)
      {
        Transform muzzle = GetMuzzle();
        var muzzleParticle = Instantiate(muzzleParticlePrefab, muzzle.position, transform.rotation);
        Destroy(muzzleParticle, 1.5f);
      }

      if (impactParticlePrefab)
      {
        var impactParticle = Instantiate(impactParticlePrefab, hitInfo.point, Quaternion.FromToRotation(Vector3.up, hitInfo.normal));
        Destroy(impactParticle, 5f);
      }

      if (useSpread)
      {
        currentSpread += spreadIncreaseEachShot;
        currentSpread = Mathf.Clamp(currentSpread, 0, maxSpread);
      }
    }

    Vector3 CalculateBulletSpread()
    {
      Quaternion fireRotation = Quaternion.LookRotation(weapon.playerCamera.forward);
      Quaternion randomRotation = Random.rotation;
      fireRotation = Quaternion.RotateTowards(fireRotation, randomRotation, Random.Range(.0f, currentSpread));
      return fireRotation * Vector3.forward;
    }
  }
}
