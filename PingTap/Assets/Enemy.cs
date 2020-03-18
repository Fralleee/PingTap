using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  public static event Action<Enemy> OnEnemyReachedPowerStone = delegate { };

  public void ReachedDestination()
  {
    OnEnemyReachedPowerStone(this);
    Destroy(gameObject, .2f);
  }
}
