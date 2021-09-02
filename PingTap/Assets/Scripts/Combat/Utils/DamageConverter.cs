namespace Fralle.PingTap
{
  public static class DamageConverter
  {
    public static float AsDamageModifier(float damageModifier, DamageType damageApplication,
      DamageController target, float weaponDamage)
    {
      return damageApplication switch
      {
        DamageType.Flat => damageModifier,
        DamageType.PercentageOfTargetCurrentHealth => target.CurrentHealth * damageModifier,
        DamageType.PercentageOfTargetMaxHealth => target.MaxHealth * damageModifier,
        DamageType.PercentageOfWeaponDamage => weaponDamage * damageModifier,
        _ => damageModifier
      };
    }
  }
}
