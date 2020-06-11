using Fralle.AI;
using Fralle.Attack.Offense;
using UnityEngine;

namespace Fralle.Attack.Effect
{
  [CreateAssetMenu(menuName = "PlayerAttack/Effect/Slow")]
  public class SlowEffect : DamageEffect
  {
    [Header("Slow specific")]
    public float slowModifier = 0.3f;

    public override void Enter(Health health)
    {
      if (health != null) health.GetComponent<EnemyNavigation>()?.AddModifier(name, slowModifier);
    }

    public override void Exit(Health health)
    {
      if (health != null) health.GetComponent<EnemyNavigation>()?.RemoveModifier(name);
    }
  }
}