using System;
using UnityEngine;

namespace Fralle.PingTap
{
  [Serializable]
  public class AIAttackPattern
  {
    public Vector2 timeBetweenAttacks;
    public Vector2 bulletsPerAttack;

    public AIAttackPattern(Vector2 timeBetweenAttacks, Vector2 bulletsPerAttack)
    {
      this.timeBetweenAttacks = timeBetweenAttacks;
      this.bulletsPerAttack = bulletsPerAttack;
    }
  }
}
