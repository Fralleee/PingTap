using UnityEngine;

public class Hitscan : Weapon
{
  [Header("Hitscan", order = 0)]
  [SerializeField] float hitForce = 3.5f;
  [SerializeField] float range = 50;
  [SerializeField] float kickbackForce = 0.15f;

  public override void Fire()
  {
    transform.localPosition -= new Vector3(0, 0, kickbackForce);
    if (Physics.Raycast(playerCamera.position, playerCamera.forward, out var hitInfo, range))
    {
      var rb = hitInfo.transform.GetComponent<Rigidbody>();
      if (rb != null) rb.AddForce(playerCamera.forward * hitForce);
    }
    recoilController?.AddRecoil();
  }
}
