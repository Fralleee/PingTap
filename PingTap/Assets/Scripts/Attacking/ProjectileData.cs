﻿using System;
using UnityEngine;

namespace Fralle
{
  [Serializable]
  public class ProjectileData
  {
    public float force = 100f;
    public bool useGravity;
    public bool kinematicOnImpact;
    public bool explodeOnMaxRange;

    public float destroyOnImpactTime;
    public float explosionRadius;
    public float explosionDamage;
    public float pushForce;
    public float range;

    [HideInInspector] public Transform launcherCamera;
  }
}