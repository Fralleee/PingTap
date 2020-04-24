using System.Collections;
using System.Collections.Generic;
using Fralle;
using UnityEngine;

public abstract class Protection : ScriptableObject
{
  public EffectProtection effectProtection;
  public abstract ProtectionResult RunProtection(DamageData data, DamageController target);
}