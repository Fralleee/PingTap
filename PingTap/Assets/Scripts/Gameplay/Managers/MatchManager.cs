using Fralle.Core.Attributes;
using Fralle.Core.Audio;
using Fralle.Core.Infrastructure;
using System;
using UnityEngine;

namespace Fralle.Gameplay
{
  public class MatchManager : Singleton<MatchManager>
  {
    public static event Action OnMatchEnd = delegate { };
    public static event Action OnDefeat = delegate { };
    public static event Action OnVictory = delegate { };
    public static event Action OnNewRound = delegate { };
    public static event Action<GameState> OnNewState = delegate { };

    public GameState gameState;
    public float prepareTime = 30f;
    public int waveCount;

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
    [Readonly] public int currentWave;

    [HideInInspector] public bool isVictory;
    [HideInInspector] public TreasureSpawner treasureSpawner;
    [HideInInspector] public bool WavesRemaining => currentWave != waveCount || enemiesAlive > 0;

    StateMachine stateMachine;
    HeadQuarters playerHome;
    Spawner enemyManager;

    //protected override void Awake()
    //{
    //  base.Awake();

    //  Enemy.OnAnyEnemyDeath += HandleEnemyDeath;
    //  LeanTween.init(1000);
    //  SetupStateMachine();
    //}

    //void Start()
    //{
    //  enemyManager = GetComponent<Spawner>();

    //  sceneCamera.gameObject.SetActive(false);

    //  playerHome = FindObjectOfType<PlayerHome>();
    //  playerHome.OnDeath += Defeat;

    //  treasureSpawner = GetComponent<TreasureSpawner>();

    //  victorySound = Instantiate(victorySoundPrefab, sceneCamera.transform.position, Quaternion.identity, transform);
    //  defeatSound = Instantiate(defeatSoundPrefab, sceneCamera.transform.position, Quaternion.identity, transform);
    //}

    //void Update()
    //{
    //  if (gameState == GameState.End) return;
    //  totalTimer += Time.deltaTime;
    //  stateMachine.Tick();
    //}

    //public void SpawnWave()
    //{
    //  currentWave++;
    //  //enemyManager.Reset();
    //  //enemiesAlive = enemyManager.enemiesToSpawn;
    //  //totalEnemies = enemyManager.enemiesToSpawn;
    //}

    //public void NewState(GameState newState)
    //{
    //  gameState = newState;
    //  OnNewState(newState);
    //}

    //public void HandleEnemyDeath(Enemy enemy)
    //{
    //  enemiesAlive--;
    //}

    //void SetupStateMachine()
    //{
    //  stateMachine = new StateMachine();

    //  var matchStatePrepare = new MatchStatePrepare();
    //  var matchStateLive = new MatchStateLive();
    //  var matchStateEnd = new MatchStateEnd();

    //  stateMachine.AddTransition(matchStatePrepare, matchStateLive, () => prepareTimer <= 0);
    //  stateMachine.AddTransition(matchStateLive, matchStatePrepare, () => enemiesAlive == 0 && WavesRemaining);
    //  stateMachine.AddTransition(matchStateLive, matchStateEnd, () => enemiesAlive == 0 && !WavesRemaining);

    //  stateMachine.SetState(matchStatePrepare);
    //  NewState(GameState.Prepare);
    //}

    //void FinishedMatch()
    //{
    //  CameraController.ConfigureCursor(false);

    //  if (sceneCamera)
    //  {
    //    Player.Disable();
    //    sceneCamera.gameObject.SetActive(true);
    //    sceneCamera.GetComponent<AudioListener>().enabled = true;
    //  }

    //  OnMatchEnd();
    //}

    //public void Victory()
    //{
    //  Debug.Log("Victory");
    //  victorySound.Spawn();
    //  FinishedMatch();
    //  OnVictory();
    //}

    //public void Defeat(PlayerHome pHome)
    //{
    //  Debug.Log("Defeat");
    //  defeatSound.Spawn();
    //  FinishedMatch();
    //  OnDefeat();
    //}

    //void OnDestroy()
    //{
    //  playerHome.OnDeath -= Defeat;
    //  Enemy.OnAnyEnemyDeath -= HandleEnemyDeath;
    //}
  }
}