using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Abilities
{
  public class TurretAbility : MonoBehaviour
  {
    [SerializeField] GameObject turretPrefab = null;
    [SerializeField] Transform cameraRig = null;

    [SerializeField] int cooldown = 0;
    [SerializeField] float throwingForce = 0f;
    [SerializeField] int turrentCount = 3;

    List<GameObject> turrets = new List<GameObject>();

    float currentCooldown;

    void Update()
    {
      HandleCooldowns();
      Deploy();
    }

    void HandleCooldowns()
    {
      if (currentCooldown > 0) currentCooldown -= Time.deltaTime;
    }

    void Deploy()
    {
      if (currentCooldown > 0) return;
      if (!Input.GetKeyDown(KeyCode.Q)) return;

      currentCooldown = cooldown;

      if (turrets.Count >= turrentCount)
      {
        Destroy(turrets[0]);
        turrets.RemoveAt(0);
      }

      var turretInstance = Instantiate(turretPrefab, cameraRig.position + cameraRig.forward, Quaternion.identity);
      turretInstance.GetComponent<Rigidbody>().AddForce(cameraRig.forward * throwingForce);

      turrets.Add(turretInstance);
    }
  }
}