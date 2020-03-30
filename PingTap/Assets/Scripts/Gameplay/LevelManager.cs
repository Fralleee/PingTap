using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
  public static event Action<LevelManager> OnDefeat = delegate { };
  public static event Action<LevelManager> OnVictory = delegate { };

  [SerializeField] float startHealth = 1000f;
  public float currentHealth;
  GameObject powerStone;

  void Awake()
  {
    currentHealth = startHealth;
    powerStone = GameObject.Find("Power Stone");
    Enemy.OnEnemyReachedPowerStone += HandleEnemyReachedPowerStone;
  }

  public void HandleEnemyReachedPowerStone(Enemy enemy)
  {
    currentHealth -= 100f;
    if (currentHealth < 0) Defeat();
  }

  void Dispose()
  {
    Enemy.OnEnemyReachedPowerStone -= HandleEnemyReachedPowerStone;
  }

  void Defeat()
  {
    Dispose();
    if (powerStone != null) Destroy(powerStone);
    OnDefeat(this);
  }
}
