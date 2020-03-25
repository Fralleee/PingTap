using System.Collections;
using UnityEngine;

namespace Fralle
{
  public class AmmoController : MonoBehaviour
  {
    [SerializeField] bool infiniteAmmo;
    [SerializeField] int maxAmmo = 30;
    [SerializeField] float reloadSpeed = 0.75f;
    [SerializeField] int currentAmmo;

    public bool isReloading { get; private set; }

    float rotationTime;

    void Start()
    {
      currentAmmo = maxAmmo;
    }

    void Update()
    {
      if (infiniteAmmo) return;
      if (isReloading)
      {
        rotationTime += Time.deltaTime;
        var spinDelta = -(Mathf.Cos(Mathf.PI * (rotationTime / reloadSpeed)) - 1f) / 2f;
        transform.localRotation = Quaternion.Euler(new Vector3(spinDelta * 360f, 0, 0));
      }
      else if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo < maxAmmo) StartCoroutine(ReloadCooldown());
    }

    public void ChangeAmmo(int change)
    {
      currentAmmo += change;
    }

    public bool HasAmmo()
    {
      if (infiniteAmmo || currentAmmo > 0) return true;
      StartCoroutine(ReloadCooldown());
      return false;
    }

    IEnumerator ReloadCooldown()
    {
      isReloading = true;
      rotationTime = 0f;
      yield return new WaitForSeconds(reloadSpeed);
      currentAmmo = maxAmmo;
      isReloading = false;
    }
  }
}