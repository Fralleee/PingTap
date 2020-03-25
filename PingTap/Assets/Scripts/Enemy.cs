using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  public static event Action<Enemy> OnEnemyReachedPowerStone = delegate { };
  public event Action<Enemy> OnDeath = delegate { };

  public void ReachedDestination()
  {
    OnEnemyReachedPowerStone(this);
    OnDeath(this);
    Destroy(gameObject, .2f);
  }
}
