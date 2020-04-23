using System.Collections;
using System.Collections.Generic;
using Fralle;
using UnityEngine;

public abstract class DamageProtection : ScriptableObject
{
  public abstract DamageData RunProtection(DamageData data, Transform target);
}