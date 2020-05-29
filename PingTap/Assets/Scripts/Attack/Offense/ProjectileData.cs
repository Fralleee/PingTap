using Fralle.Attack.Effect;
using Fralle.Player;
using System;
using UnityEngine;

namespace Fralle.Attack.Offense
{
  [Serializable]
  public class ProjectileData
  {
    public float force = 100f;
    public bool useGravity;
    public bool kinematicOnImpact;
    public bool explodeOnMaxRange;

    public float explodeOnImpactTime;
    public float explodeOnTime;
    public float explosionRadius;
    public float pushForce;
    public float range;

    [HideInInspector] public PlayerMain player;
    [HideInInspector] public float damage;
    [HideInInspector] public Vector3 forward;
    [HideInInspector] public Element element;
    [HideInInspector] public DamageEffect[] damageEffects;
  }
}