using Fralle.Core;
using UnityEngine;

namespace Fralle.PingTap
{
  public class Hitbox : MonoBehaviour, IDisableOnDeath
  {
    public HitArea HitArea = HitArea.Chest;

    void OnCollisionEnter(Collision collision)
    {
      if (enabled && collision.impulse.magnitude > 30)
      {
        DamageHelper.CollisionHit(this, collision);
      }
    }

  }
}
