using System;
using Fralle;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
  public static event Action<MatchManager> OnDefeat = delegate { };
  public static event Action<MatchManager> OnVictory = delegate { };

  public GameState gameState;
  public float prepareTime = 30f;

  [Space(10)]
  [Readonly] public int enemiesAlive;
  [Readonly] public float prepareTimer;
  [Readonly] public float totalTimer;
  [Readonly] public float roundTimer;

  [HideInInspector] public bool isVictory;

  StateController stateController;

  void Awake()
  {
    stateController = GetComponent<StateController>();

    Enemy.OnAnyEnemyDeath += HandleEnemyDeath;
    Nexus.OnDeath += Defeat;
    WaveManager.OnWavesComplete += HandleWavesComplete;
  }

  void Update()
  {
    if (gameState == GameState.End) return;
    totalTimer += Time.deltaTime;
  }

  public void HandleEnemyDeath(Enemy enemy)
  {
    enemiesAlive--;
  }

  void HandleWavesComplete(WaveManager waveManager)
  {
    Victory();
  }

  void Dispose()
  {
    Enemy.OnAnyEnemyDeath -= HandleEnemyDeath;
    Nexus.OnDeath -= Defeat;
  }

  void Victory()
  {
    stateController.enabled = false;

    Dispose();
    OnVictory(this);
  }

  void Defeat(Nexus nexus)
  {
    Dispose();
    OnDefeat(this);
  }
}
