using Fralle;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack/Damage Protection/Back attack")]
public class BackAttack : DamageProtection
{
  public override DamageData RunProtection(DamageData data, Transform target)
  {
    if (data.hitAngle < 90 || data.hitAngle > 270)
    {
      data.damage = 0;
      data.bodyPartType = BodyPartType.Minor;
    }
    return data;
  }

}