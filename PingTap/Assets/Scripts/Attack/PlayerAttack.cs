using Fralle.Core.Extensions;
using Fralle.Resource;
using System.Linq;
using UnityEngine;

namespace Fralle.Attack
{
  public class PlayerAttack : MonoBehaviour
  {
    public Weapon equippedWeapon;

    [SerializeField] Transform weaponHolder;
    [SerializeField] Transform playerCamera;

    InventoryController inventory;
    Weapon[] weapons;

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
    }

    void EquipWeapon(Weapon weapon)
    {
      if (equippedWeapon && equippedWeapon.weaponName == weapon.weaponName) return;
      if (equippedWeapon) Destroy(equippedWeapon.gameObject);

      equippedWeapon = Instantiate(weapon, weaponHolder.transform.position.With(y: -0.5f), transform.rotation);
      equippedWeapon.Equip(weaponHolder, playerCamera);
    }

    void SwapWeapon()
    {
      for (var i = 1; i <= weapons.Length; i++)
        if (Input.GetKeyDown("" + i))
          EquipWeapon(weapons[i - 1]);
    }
  }
}