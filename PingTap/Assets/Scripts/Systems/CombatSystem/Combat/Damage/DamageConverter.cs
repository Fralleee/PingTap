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
        case DamageType.PercentageOfTargetCurrentHealth: return target.currentHealth * damageModifier;
        case DamageType.PercentageOfTargetMaxHealth: return target.maxHealth * damageModifier;
        case DamageType.PercentageOfWeaponDamage: return weaponDamage * damageModifier;
        default: return damageModifier;
      }
    }
  }
}