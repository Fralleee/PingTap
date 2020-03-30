using UnityEngine;

namespace Fralle
{
  public class WeaponManager : MonoBehaviour
  {
    [SerializeField] Weapon[] weapons;

    [SerializeField] float swaySize = 0.004f;
    [SerializeField] float swaySmooth = 25f;

    [SerializeField] new Rigidbody rigidbody;
    [SerializeField] Transform weaponHolder;
    [SerializeField] Transform playerCamera;
    [SerializeField] Transform swayHolder;

    bool hasEquippedWeapon;
    Weapon equippedWeapon;

    float oldRBVelocityY;
    float bounceBackVelocityY;
    float bounceBackThreshold = 0.1f;

    void Start()
    {
      if (weapons.Length > 0) EquipWeapon(weapons[0]);
    }

    void Update()
    {
      SwapWeapon();

      if (!hasEquippedWeapon) return;

      float velocityY = rigidbody.velocity.y;
      if (oldRBVelocityY != 0 && velocityY == 0) bounceBackVelocityY = 5f;
      if (bounceBackVelocityY > bounceBackThreshold && velocityY == 0)
      {
        velocityY = bounceBackVelocityY;
        Mathf.SmoothDamp(bounceBackVelocityY, 0, ref bounceBackVelocityY, 1f);
      }

      oldRBVelocityY = rigidbody.velocity.y;
      Vector2 delta = -new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") + velocityY);

      if (!Cursor.visible)
      {
        swayHolder.localPosition = Vector3.Lerp(swayHolder.localPosition, Vector3.zero, swaySmooth * Time.deltaTime);
        swayHolder.localPosition += (Vector3)delta * swaySize;
      }
    }

    void EquipWeapon(Weapon weapon)
    {
      if (equippedWeapon && equippedWeapon.weaponName == weapon.weaponName) return;
      if (equippedWeapon) Destroy(equippedWeapon.gameObject);

      equippedWeapon = Instantiate(weapon, transform.position.With(y: -0.5f), transform.rotation);
      equippedWeapon?.Equip(weaponHolder, playerCamera);
      hasEquippedWeapon = equippedWeapon != null;
    }

    void SwapWeapon()
    {
      for (int i = 1; i <= weapons.Length; i++)
        if (Input.GetKeyDown("" + i))
          EquipWeapon(weapons[i - 1]);
    }
  }
}