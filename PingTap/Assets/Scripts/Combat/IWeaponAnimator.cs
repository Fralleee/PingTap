namespace Fralle.PingTap
{
  public interface IWeaponAnimator
  {
    void AnimateEquip(Combatant combatant, float duration);
    void AnimateUnequip(Combatant combatant, float duration);
  }
}
