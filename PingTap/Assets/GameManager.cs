using Fralle;
using Fralle.Core.Attributes;
using Fralle.Core.Audio;
using Fralle.Core.Infrastructure;
using Fralle.Core.Interfaces;
using Fralle.FpsController;
using Fralle.Gameplay;
using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
  public static event Action OnMatchEnd = delegate { };
  public static event Action OnDefeat = delegate { };
  public static event Action OnVictory = delegate { };
  public static event Action OnNewWave = delegate { };
  public static event Action<GameState> OnSetState = delegate { };

  public GameState gameState;
  public IState currentState;

  public float prepareTime = 30f;

  public int waveCount;
  public int currentWave;
  public float zombieLevelIncreaseFactor = 1.2f;
  public SpawnWave wave;

  AudioManager audioManager;

  StateMachine stateMachine;
  Spawner spawner;
  PlayerHome playerHome;


  [Header("Cameras")]
  [SerializeField] Camera sceneCamera = null;

  [Header("Audio")]
  [SerializeField] AudioEvent victorySound = null;
  [SerializeField] AudioEvent defeatSound = null;

  [Space(10)]
  [Readonly] public int enemiesAlive;
  [Readonly] public int totalEnemies;
  [Readonly] public float prepareTimer;
  [Readonly] public float totalTimer;
  [Readonly] public float waveTimer;

  [HideInInspector] public bool isVictory;
  [HideInInspector] public TreasureSpawner treasureSpawner;


  protected override void Awake()
  {
    base.Awake();

    audioManager = GetComponent<AudioManager>();
    spawner = GetComponent<Spawner>();

    SetupStateMachine();
  }

  void Start()
  {
    spawner.SetSpawnDefinition(wave);
    audioManager.Play(victorySound);
  }

  void SetupStateMachine()
  {
    stateMachine = new StateMachine();

    var matchStatePrepare = new MatchStatePrepare();
    var matchStateLive = new MatchStateLive();
    var matchStateEnd = new MatchStateEnd();

    stateMachine.AddTransition(matchStatePrepare, matchStateLive, () => prepareTimer <= 0);
    //stateMachine.AddTransition(matchStateLive, matchStatePrepare, () => enemiesAlive == 0 && WavesRemaining);
    //stateMachine.AddTransition(matchStateLive, matchStateEnd, () => enemiesAlive == 0 && !WavesRemaining);

    stateMachine.SetState(matchStatePrepare);
    SetState(GameState.Prepare);
  }

  void Update()
  {
    if (gameState == GameState.End) return;
    totalTimer += Time.deltaTime;
    stateMachine.Tick();
  }

  void FinishedMatch()
  {
    CameraController.ConfigureCursor(false);

    if (sceneCamera)
    {
      Player.Disable();
      sceneCamera.gameObject.SetActive(true);
      sceneCamera.GetComponent<AudioListener>().enabled = true;
    }

    OnMatchEnd();
  }

  public void SetState(GameState newState)
  {
    gameState = newState;
    OnSetState(newState);
  }


  public void Victory()
  {
    Debug.Log("Victory");
    audioManager.Play(victorySound);
    FinishedMatch();
    OnVictory();
  }

  public void Defeat(PlayerHome pHome)
  {
    Debug.Log("Defeat");
    audioManager.Play(defeatSound);
    FinishedMatch();
    OnDefeat();
  }
}
