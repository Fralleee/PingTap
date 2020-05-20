using Fralle.Attack.Effect;
using UnityEngine;

namespace Fralle.Attack.Offense
{
  public class Damage
  {
    public Player player;
    public Element element;
    public DamageEffect[] effects;

    public Vector3 position;
    public Vector3 force;

    public float hitAngle;
    public float damageAmount;

    public Damage()
    {
      effects = new DamageEffect[0];
    }
  }
}