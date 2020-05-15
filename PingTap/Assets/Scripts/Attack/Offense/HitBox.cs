using UnityEngine;

namespace Fralle.Attack.Offense
{
  [RequireComponent(typeof(SphereCollider))]
  public class HitBox : MonoBehaviour
  {
    public HitBoxType hitBoxType;
    Health health;

    void Awake()
    {
      health = GetComponentInParent<Health>();
      if (health == null) Debug.LogError($"{gameObject.name}s hitbox is missing Health in parent");
    }

    public void ApplyHit(Damage damage)
    {
      health.ReceiveAttack(damage);
    }
  }
}