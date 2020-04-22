using System;
using UnityEngine;

namespace Fralle
{
  public class DamageData
  {
    public Player player;
    public DamageType damageType;
    public BodyPartType bodyPartType;
    public Vector3 position;
    public float damage;

    public DamageData WithBodyPartModifier()
    {
      switch (bodyPartType)
      {
        case BodyPartType.Body: return this;
        case BodyPartType.Head:
          damage *= 2.5f;
          return this;
        case BodyPartType.Minor:
          damage *= 0.75f;
          return this;
        default:
          return this;
      }
    }
  }
}