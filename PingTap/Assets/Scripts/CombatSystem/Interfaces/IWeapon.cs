using CombatSystem.Combat;
using CombatSystem.Enums;

namespace CombatSystem.Interfaces
{
  public interface IWeapon
  {
    void Equip(Combatant combatant);
    void ChangeWeaponAction(Status newActiveWeaponAction);
  }
}