using CombatSystem.Combat.Damage;
using CombatSystem.Enums;
using UnityEngine;

namespace CombatSystem.Defense
{
  public abstract class Protection : ScriptableObject
  {
    public EffectProtection EffectProtection;
    public abstract ProtectionResult RunProtection(DamageData damageData, DamageController damageController);
  }
}