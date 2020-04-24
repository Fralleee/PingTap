using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;

namespace Fralle
{
  public class WeaponManager : MonoBehaviour
  {
    public Weapon equippedWeapon;

    [SerializeField] float swaySize = 0.004f;
    [SerializeField] float swaySmooth = 25f;
    [SerializeField] float idleSmooth = 1f;

    [SerializeField] Transform weaponHolder;
    [SerializeField] Transform playerCamera;
    [SerializeField] Transform swayHolder;

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

      Vector2 delta = -new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // + velocityY);
      if (!Cursor.visible && delta.magnitude > 0)
      {
        swayHolder.localPosition = Vector3.Lerp(swayHolder.localPosition, Vector3.zero, swaySmooth * Time.deltaTime);
        swayHolder.localPosition += (Vector3)delta * swaySize;
      }
      else
      {
        swayHolder.localPosition = Vector3.Lerp(swayHolder.localPosition, nextIdlePosition, idleSmooth * Time.deltaTime);
        if(Vector3.Distance(swayHolder.localPosition, nextIdlePosition) < 0.005f) NewIdlePosition();
      }
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


    public void OnPrimaryFire(InputValue value)
    {
      Debug.Log("OnPrimaryFire" + value);
    }

    public void OnSecondaryFire(InputValue value)
    {
      Debug.Log("OnSecondaryFire" + value);
    }
  }
}