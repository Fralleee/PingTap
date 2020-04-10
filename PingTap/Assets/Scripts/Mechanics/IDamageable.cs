namespace Fralle
{
  public interface IDamageable
  {
    void TakeDamage(DamageData data);
    void Death(DamageData data);
  }
}