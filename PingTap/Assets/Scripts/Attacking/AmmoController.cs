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

    bool isReloading;
    float rotationTime;
    Weapon weapon;

    void Awake()
    {
      weapon = GetComponent<Weapon>();
    }

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
      else if (Input.GetKeyDown(KeyCode.R) && !weapon.performingAction && currentAmmo < maxAmmo) StartCoroutine(ReloadCooldown());
    }

    public void ChangeAmmo(int change)
    {
      currentAmmo += change;
    }

    public bool HasAmmo(int requiredAmmo = 1)
    {
      if (infiniteAmmo || currentAmmo >= requiredAmmo) return true;
      StartCoroutine(ReloadCooldown());
      return false;
    }

    IEnumerator ReloadCooldown()
    {
      weapon.performingAction = true;
      isReloading = true;
      rotationTime = 0f;
      yield return new WaitForSeconds(reloadSpeed);
      currentAmmo = maxAmmo;
      weapon.performingAction = false;
      isReloading = false;
    }
  }
}