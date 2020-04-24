using Fralle;

public static class DamageConverter
{
  public static float AsDamageModifier(float damageModifier, DamageApplication damageApplication, DamageController target, float weaponDamage)
  {
    switch (damageApplication)
    {
      case DamageApplication.Flat: return damageModifier;
      case DamageApplication.PercentageOfTargetCurrentHealth: return target.currentHealth * damageModifier;
      case DamageApplication.PercentageOfTargetMaxHealth: return target.maxHealth * damageModifier;
      case DamageApplication.PercentageOfWeaponDamage: return weaponDamage * damageModifier;
      default: return damageModifier;
    }
  }
}