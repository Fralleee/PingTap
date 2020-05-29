using Fralle.AI;
using Fralle.Core.Attributes;
using System;
using UnityEngine;

namespace Fralle.Gameplay
{
  public class MatchManager : MonoBehaviour
  {
    public static event Action<MatchManager> OnMatchEnd = delegate { };
    public static event Action<MatchManager, PlayerStats> OnDefeat = delegate { };
    public static event Action<MatchManager, PlayerStats> OnVictory = delegate { };
    public static event Action<MatchManager> OnNewRound = delegate { };
    public static event Action<GameState> OnNewState = delegate { };

    public GameState gameState;
    public float prepareTime = 30f;

    [Header("Cameras")]
    [SerializeField] Camera sceneCamera;

    [Space(10)]
    [Readonly] public int enemiesAlive;
    [Readonly] public int totalEnemies;
    [Readonly] public float prepareTimer;
    [Readonly] public float totalTimer;
    [Readonly] public float waveTimer;

    [HideInInspector] public bool isVictory;
    [HideInInspector] public TreasureSpawner treasureSpawner;

    MatchState matchState;
    PlayerHome playerHome;

    void Awake()
    {
      matchState = GetComponent<MatchState>();
      sceneCamera.gameObject.SetActive(false);

      playerHome = FindObjectOfType<PlayerHome>();
      playerHome.OnDeath += Defeat;

      treasureSpawner = GetComponent<TreasureSpawner>();

      Enemy.OnAnyEnemyDeath += HandleEnemyDeath;
      WaveManager.OnWavesComplete += HandleWavesComplete;
    }

    void Update()
    {
      if (gameState == GameState.End) return;
      totalTimer += Time.deltaTime;
    }

    public void NewState(GameState newState)
    {
      gameState = newState;
      OnNewState(newState);
    }

    public void NewWave(int enemyCount)
    {
      enemiesAlive = enemyCount;
      totalEnemies = enemyCount;
      OnNewRound(this);
    }

    public void HandleEnemyDeath(Enemy enemy)
    {
      enemiesAlive--;
    }

    void HandleWavesComplete(WaveManager waveManager)
    {
      Victory();
    }

    PlayerStats FinishedMatch()
    {
      var stats = new PlayerStats();
      var player = FindObjectOfType<Player>();
      if (player) stats = player.stats;

      if (sceneCamera)
      {
        Player.Disable();
        sceneCamera.gameObject.SetActive(true);
      }

      matchState.enabled = false;
      OnMatchEnd(this);
      return stats;
    }

    void Victory()
    {
      var stats = FinishedMatch();
      OnVictory(this, stats);
    }

    void Defeat(PlayerHome playerHome)
    {
      var stats = FinishedMatch();
      OnDefeat(this, stats);
    }

    void OnDestroy()
    {
      playerHome.OnDeath -= Defeat;
      Enemy.OnAnyEnemyDeath -= HandleEnemyDeath;
      WaveManager.OnWavesComplete -= HandleWavesComplete;
    }
  }
}