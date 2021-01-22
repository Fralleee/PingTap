using CombatSystem.Combat;
using CombatSystem.Enums;

namespace CombatSystem.Interfaces
{
	public interface IWeapon
	{
		void Equip(Combatant combatant, bool shouldAnimate = true);
		void ChangeWeaponAction(Status newActiveWeaponAction);
	}
}
