using UnityEngine;

namespace Fralle.Attack
{
  [CreateAssetMenu(menuName = "Attack/Effect/Slow")]
  public class SlowEffect : DamageEffect
  {
    [Header("Slow specific")] public float slowModifier = 0.3f;

    public override void Enter(Health health)
    {
      health.GetComponent<AgentNavigation>().AddModifier(name, slowModifier);
    }

    public override void Exit(Health health)
    {
      health.GetComponent<AgentNavigation>().RemoveModifier(name);
    }
  }
}