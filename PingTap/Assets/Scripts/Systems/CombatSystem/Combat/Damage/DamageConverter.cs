namespace CombatSystem.Combat.Damage
{
  public static class DamageConverter
  {
    public static float AsDamageModifier(float damageModifier, DamageType damageApplication,
      DamageController target, float weaponDamage)
    {
      switch (damageApplication)
      {
        case DamageType.Flat: return damageModifier;
        case DamageType.PercentageOfTargetCurrentHealth: return target.CurrentHealth * damageModifier;
        case DamageType.PercentageOfTargetMaxHealth: return target.MaxHealth * damageModifier;
        case DamageType.PercentageOfWeaponDamage: return weaponDamage * damageModifier;
        default: return damageModifier;
      }
    }
  }
}