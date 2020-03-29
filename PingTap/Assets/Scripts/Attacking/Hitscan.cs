using UnityEngine;

public class Hitscan : WeaponAction
{
  [Header("Hitscan", order = 0)]
  [SerializeField] float range = 50;
  [SerializeField] float spreadIncreseEachShot = 3.5f;
  [SerializeField] float maxSpread = 3.5f;
  [SerializeField] float pushForce = 3.5f;

  public override void Fire()
  {
    if (Physics.Raycast(weapon.playerCamera.position, weapon.playerCamera.forward, out var hitInfo, range))
    {
      var rb = hitInfo.transform.GetComponent<Rigidbody>();
      if (rb != null) rb.AddForce(weapon.playerCamera.forward * pushForce);
    }
  }
}
