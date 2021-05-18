using CombatSystem.Combat.Damage;

namespace CombatSystem.Interfaces
{
  public interface IDamageable
  {
    void TakeDamage(DamageData damageData);
  }
}