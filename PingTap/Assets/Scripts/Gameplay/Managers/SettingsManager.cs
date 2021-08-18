using Fralle.Core;
using UnityEngine;

namespace Fralle.Gameplay
{
  public class SettingsManager : MonoBehaviour
  {
    [Space(10)]
    [Readonly] public int EnemiesSpawned;
    [Readonly] public int EnemiesKilled;
    [Readonly] public int TotalEnemies;
    [Readonly] public float PrepareTimer;
    [Readonly] public float TotalTimer;
    [Readonly] public float WaveTimer;

    [SerializeField] float prepareTime = 5f;

    void Awake()
    {
    }

    void Update()
    {
      TotalTimer += Time.deltaTime;
    }

    public void ResetPreparationTimer()
    {
      prepareTime = PrepareTimer;
    }

  }
}
