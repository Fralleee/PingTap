using Fralle.Core.Attributes;
using Fralle.Core.Audio;
using Fralle.Core.Infrastructure;
using Fralle.Core.Interfaces;
using Fralle.FpsController;
using System;
using UnityEngine;

namespace Fralle.Gameplay
{
  public class GameManager : Singleton<GameManager>
  {
    public static event Action OnMatchEnd = delegate { };
    public static event Action OnDefeat = delegate { };
    public static event Action OnVictory = delegate { };
    public static event Action OnNewWave = delegate { };
    public static event Action<GameState> OnSetState = delegate { };

    public GameState gameState;
    public IState currentState;

    public float prepareTime = 5f;

    [Header("Cameras")]
    [SerializeField] Camera sceneCamera;

    [Header("Audio")]
    [SerializeField] AudioEvent victorySound;
    [SerializeField] AudioEvent defeatSound;

    [Space(10)]
    [Readonly] public int enemiesSpawned;
    [Readonly] public int enemiesKilled;
    [Readonly] public int totalEnemies;
    [Readonly] public float prepareTimer;
    [Readonly] public float totalTimer;
    [Readonly] public float waveTimer;

    [HideInInspector] public bool isVictory;
    [HideInInspector] public TreasureSpawner treasureSpawner;
    [HideInInspector] public EnemyManager enemyManager;
    [HideInInspector] public HeadQuarters playerHome;

    AudioManager audioManager;
    StateMachine stateMachine;

    bool endMatch;

    protected override void Awake()
    {
      base.Awake();

      audioManager = GetComponent<AudioManager>();
      enemyManager = GetComponent<EnemyManager>();
      playerHome = FindObjectOfType<HeadQuarters>();

      SetupStateMachine();

      playerHome.OnDeath += HandleHomeDeath;
    }

    void SetupStateMachine()
    {
      stateMachine = new StateMachine();

      var matchStatePrepare = new MatchStatePrepare();
      var matchStateLive = new MatchStateLive();
      var matchStateEnd = new MatchStateEnd();

      stateMachine.AddTransition(matchStatePrepare, matchStateLive, () => prepareTimer <= 0);
      stateMachine.AddTransition(matchStateLive, matchStatePrepare, () => enemyManager.AllEnemiesDead && enemyManager.waves.Count > 0);
      stateMachine.AddTransition(matchStateLive, matchStateEnd, () => endMatch || enemyManager.AllEnemiesDead && enemyManager.waves.Count == 0);

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

    public void Defeat()
    {
      Debug.Log("Defeat");
      audioManager.Play(defeatSound);
      FinishedMatch();
      OnDefeat();
    }

    void HandleHomeDeath(HeadQuarters home)
    {
      endMatch = true;
    }
  }
}