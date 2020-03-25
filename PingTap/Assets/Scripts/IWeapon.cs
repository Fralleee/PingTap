using UnityEngine;

public interface IWeapon
{
  void Equip(Transform weaponHolder, Transform playerCamera);
  void Fire();
}
