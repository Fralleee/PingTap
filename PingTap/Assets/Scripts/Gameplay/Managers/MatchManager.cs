using Fralle.AI;
using Fralle.Core.Attributes;
using Fralle.Core.Audio;
using Fralle.Core.Infrastructure;
using Fralle.Movement;
using Fralle.Player;
using System;
using UnityEngine;

namespace Fralle.Gameplay
{
  public class MatchManager : Singleton<MatchManager>
  {
    public static event Action OnMatchEnd = delegate { };
    public static event Action<PlayerStats> OnDefeat = delegate { };
    public static event Action<PlayerStats> OnVictory = delegate { };
    public static event Action OnNewRound = delegate { };
    public static event Action<GameState> OnNewState = delegate { };

    public GameState gameState;
    public float prepareTime = 30f;

    [Header("Cameras")]
    [SerializeField] Camera sceneCamera = null;

    [Header("Audio")]
    [SerializeField] SpawnSounds victorySoundPrefab = null;
    [SerializeField] SpawnSounds defeatSoundPrefab = null;
    SpawnSounds victorySound;
    SpawnSounds defeatSound;

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

    protected override void Awake()
    {
      base.Awake();

      Enemy.OnAnyEnemyDeath += HandleEnemyDeath;
      WaveManager.OnWavesComplete += HandleWavesComplete;
      LeanTween.init(1000);
    }

    void Start()
    {
      matchState = GetComponent<MatchState>();
      sceneCamera.gameObject.SetActive(false);

      playerHome = FindObjectOfType<PlayerHome>();
      playerHome.OnDeath += Defeat;

      treasureSpawner = GetComponent<TreasureSpawner>();

      victorySound = Instantiate(victorySoundPrefab, sceneCamera.transform.position, Quaternion.identity, transform);
      defeatSound = Instantiate(defeatSoundPrefab, sceneCamera.transform.position, Quaternion.identity, transform);
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
      OnNewRound();
    }

    public void HandleEnemyDeath(Enemy enemy)
    {
      enemiesAlive--;
    }

    void HandleWavesComplete()
    {
      Victory();
    }

    PlayerStats FinishedMatch()
    {
      var stats = new PlayerStats();
      var player = FindObjectOfType<PlayerMain>();
      if (player) stats = player.stats;

      PlayerMouseLook.ConfigureCursor(false);

      if (sceneCamera)
      {
        PlayerMain.Disable();
        sceneCamera.gameObject.SetActive(true);
        sceneCamera.GetComponent<AudioListener>().enabled = true;
      }

      matchState.enabled = false;
      OnMatchEnd();
      return stats;
    }

    void Victory()
    {
      Debug.Log("Victory");
      victorySound.Spawn();
      var stats = FinishedMatch();
      OnVictory(stats);
    }

    void Defeat(PlayerHome pHome)
    {
      Debug.Log("Defeat");
      defeatSound.Spawn();
      var stats = FinishedMatch();
      OnDefeat(stats);
    }

    void OnDestroy()
    {
      playerHome.OnDeath -= Defeat;
      Enemy.OnAnyEnemyDeath -= HandleEnemyDeath;
      WaveManager.OnWavesComplete -= HandleWavesComplete;
    }
  }
}