using System;
using UnityEngine;

namespace Fralle
{
  [Serializable]
  public class ProjectileData
  {
    public Player player;
    public Transform muzzle;
    public float force = 100f;
    public bool useGravity;
    public bool kinematicOnImpact;
    public bool explodeOnMaxRange;

    public float explodeOnImpactTime;
    public float explodeOnTime;
    public float explosionRadius;
    public float explosionDamage;
    public float pushForce;
    public float range;

    [HideInInspector] public Vector3 forward;
  }
}