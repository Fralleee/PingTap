using Fralle.Attack.Offense;
using UnityEngine;

namespace Fralle.Attack.Defense
{
  public abstract class Protection : ScriptableObject
  {
    public EffectProtection effectProtection;
    public abstract ProtectionResult RunProtection(Damage data, Health target);
  }
}