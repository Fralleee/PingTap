using Sirenix.OdinInspector;
using UnityEngine;

namespace Fralle.Gameplay
{
  public class SettingsManager : MonoBehaviour
  {
    [Space(10)]
    [ReadOnly] public int EnemiesSpawned;
    [ReadOnly] public int EnemiesKilled;
    [ReadOnly] public int TotalEnemies;
    [ReadOnly] public float PrepareTimer;
    [ReadOnly] public float TotalTimer;
    [ReadOnly] public float WaveTimer;

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
