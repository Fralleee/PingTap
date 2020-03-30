namespace Fralle
{
  public interface IDamageable
  {
    void TakeDamage(float rawDamage);
    void Death();
  }
}