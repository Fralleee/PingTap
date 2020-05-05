using Fralle.Core.Extensions;
using Fralle.Resource;
using System.Linq;
using UnityEngine;

namespace Fralle.Attack
{
  public class Attack : MonoBehaviour
  {
    public Weapon equippedWeapon;
    public Transform weaponRig;

    [SerializeField] float swaySize = 0.004f;
    [SerializeField] float swaySmooth = 25f;
    [SerializeField] float idleSmooth = 1f;

    [SerializeField] Transform weaponHolder;
    [SerializeField] Transform playerCamera;

    bool hasEquippedWeapon;
    InventoryController inventory;
    Weapon[] weapons;

    Vector3 nextIdlePosition = Vector3.zero;

    void Awake()
    {
      inventory = GetComponentInParent<InventoryController>();
    }

    void Start()
    {
      weapons = inventory.items.Where(x => x.GetType() == typeof(Weapon)).Select(x => (Weapon)x).ToArray();
      if (weapons.Length > 0) EquipWeapon(weapons[0]);
    }

    void Update()
    {
      SwapWeapon();

      if (!hasEquippedWeapon) return;

      var delta = -new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // + velocityY);
      if (!Cursor.visible && delta.magnitude > 0) PerformSway(delta);
      else PerformIdle();
    }

    void PerformSway(Vector2 delta)
    {
      weaponRig.localPosition = Vector3.Lerp(weaponRig.localPosition, Vector3.zero, swaySmooth * Time.deltaTime);
      weaponRig.localPosition += (Vector3)delta * swaySize;
    }

    void PerformIdle()
    {
      weaponRig.localPosition = Vector3.Lerp(weaponRig.localPosition, nextIdlePosition, idleSmooth * Time.deltaTime);
      if (Vector3.Distance(weaponRig.localPosition, nextIdlePosition) < 0.005f) NewIdlePosition();
    }

    void NewIdlePosition()
    {
      nextIdlePosition = Random.insideUnitCircle * 0.01f;
    }

    void EquipWeapon(Weapon weapon)
    {
      if (equippedWeapon && equippedWeapon.weaponName == weapon.weaponName) return;
      if (equippedWeapon) Destroy(equippedWeapon.gameObject);

      equippedWeapon = Instantiate(weapon, transform.position.With(y: -0.5f), transform.rotation);
      equippedWeapon.Equip(weaponHolder, playerCamera);
      hasEquippedWeapon = equippedWeapon != null;
    }

    void SwapWeapon()
    {
      for (var i = 1; i <= weapons.Length; i++)
        if (Input.GetKeyDown("" + i))
          EquipWeapon(weapons[i - 1]);
    }
  }
}