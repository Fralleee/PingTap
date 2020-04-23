using System;
using UnityEngine;

namespace Fralle
{
  public class DamageData
  {
    public Player player;
    public Element element;
    public DamageEffect[] effects;
    public BodyPartType bodyPartType = BodyPartType.Major;
    public Vector3 position;
    public float hitAngle;
    public float damage;

    public DamageData()
    {
      effects = new DamageEffect[0];
    }

    public DamageData WithBodyPartModifier()
    {
      switch (bodyPartType)
      {
        case BodyPartType.Major: return this;
        case BodyPartType.Nerve:
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