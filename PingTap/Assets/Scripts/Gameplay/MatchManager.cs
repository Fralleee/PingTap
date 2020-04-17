using System;
using Fralle;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
  public static event Action<MatchManager> OnDefeat = delegate { };
  public static event Action<MatchManager> OnVictory = delegate { };
  public static event Action<MatchManager> OnNewRound = delegate { };

  public GameState gameState;
  public float prepareTime = 30f;

  [Header("UI")]
  [SerializeField] GameObject roundsUI;
  [SerializeField] GameObject timersUI;
  [SerializeField] GameObject enemiesUI;

  [Space(10)]
  [Readonly] public int enemiesAlive;
  [Readonly] public int totalEnemies;
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

  void Start()
  {
    SetupUI();
  }

  void Update()
  {
    if (gameState == GameState.End) return;
    totalTimer += Time.deltaTime;
  }

  void SetupUI()
  {
    var ui = new GameObject("UI");
    ui.transform.parent = transform;
    Instantiate(roundsUI, ui.transform);
    Instantiate(timersUI, ui.transform);
    Instantiate(enemiesUI, ui.transform);
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

  void Victory()
  {
    stateController.enabled = false;

    OnVictory(this);
  }

  void Defeat(Nexus nexus)
  {
    OnDefeat(this);
  }

  void OnDestroy()
  {
    Enemy.OnAnyEnemyDeath -= HandleEnemyDeath;
    Nexus.OnDeath -= Defeat;
    WaveManager.OnWavesComplete -= HandleWavesComplete;
  }

}
