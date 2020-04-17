using Fralle;
using UnityEngine;

public class Hitscan : WeaponAction
{
  [Header("Hitscan")]
  [SerializeField] float range = 50;
  [SerializeField] float pushForce = 3.5f;
  [SerializeField] GameObject impactParticlePrefab;
  [SerializeField] GameObject muzzleParticlePrefab;
  [SerializeField] GameObject lineRendererPrefab;

  [Header("Spread")]
  [SerializeField] bool useSpread;
  [SerializeField] float spreadIncreaseEachShot = 0.75f;
  [SerializeField] float maxSpread = 5f;
  [SerializeField] float recovery = 5f;

  [Readonly] public float currentSpread;

  internal override void Update()
  {
    base.Update();
    if (currentSpread == 0) return;
    currentSpread -= Time.deltaTime * recovery;
    currentSpread = Mathf.Clamp(currentSpread, 0, maxSpread);
  }

  public override void Fire()
  {
    Vector3 forward = weapon.playerCamera.forward;
    if (useSpread) forward = CalculateBulletSpread();

    int layerMask = ~LayerMask.GetMask("Corpse");
    if (Physics.Raycast(weapon.playerCamera.position, forward, out var hitInfo, range, layerMask))
    {
      var rb = hitInfo.transform.GetComponent<Rigidbody>();
      if (rb != null) rb.AddForce(weapon.playerCamera.forward * pushForce);

      var damageable = hitInfo.transform.GetComponentInParent<DamageController>();
      if (damageable != null) damageable.TakeDamage(new DamageData() {player = player, damage = Damage});

      Transform muzzle = GetMuzzle();
      BulletTrace(muzzle.position, hitInfo.point);

      if (muzzleParticlePrefab)
      {
        GameObject muzzleParticle = Instantiate(muzzleParticlePrefab, muzzle.position, transform.rotation, muzzle);
        Destroy(muzzleParticle, 1.5f);
      }

      if (impactParticlePrefab)
      {
        GameObject impactParticle = Instantiate(impactParticlePrefab, hitInfo.point, Quaternion.FromToRotation(Vector3.up, hitInfo.normal));
        Destroy(impactParticle, 5f);
      }

      if (!useSpread) return;
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

  void BulletTrace(Vector3 origin, Vector3 target)
  {
    if (!lineRendererPrefab) return;
    GameObject lineRendererInstance = Instantiate(lineRendererPrefab);
    var lineRenderer = lineRendererInstance.GetComponent<LineRenderer>();
    lineRenderer.SetPosition(0, origin);
    lineRenderer.SetPosition(1, target);
  }
}
