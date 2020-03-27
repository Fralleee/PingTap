using UnityEngine;

public class Hitscan : Weapon
{
  [Header("Hitscan", order = 0)]
  [SerializeField] float range = 50;
  [SerializeField] float spreadIncreseEachShot = 3.5f;
  [SerializeField] float maxSpread = 3.5f;
  [SerializeField] float pushForce = 3.5f;

  public override void Fire()
  {
    transform.localPosition -= new Vector3(0, 0, kickbackForce);
    if (Physics.Raycast(playerCamera.position, playerCamera.forward, out var hitInfo, range))
    {
      var rb = hitInfo.transform.GetComponent<Rigidbody>();
      if (rb != null) rb.AddForce(playerCamera.forward * pushForce);
    }
    recoilController?.AddRecoil();
  }
}
