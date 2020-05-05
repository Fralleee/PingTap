using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveContainerUi : MonoBehaviour
{
  [SerializeField] WaveStatusUi waveStatusPrefab;
  readonly List<WaveStatusUi> waves = new List<WaveStatusUi>();
  WaveStatusUi currentWave;

  void Awake()
  {
    WaveManager.OnNewSchema += HandleNewSchema;
    WaveManager.OnNewWave += HandleNewWave;
    WaveManager.OnWaveProgress += HandleWaveProgress;
    MatchManager.OnVictory += HandleVictory;
  }

  void HandleNewSchema(WaveManager waveManager)
  {
    waves.ForEach(x => Destroy(x.gameObject));
    waves.Clear();

    var army = waveManager.GetCurrentArmy;
    for (var i = 0; i < army.MaxRounds; i++)
    {
      var wave = Instantiate(waveStatusPrefab, transform);
      waves.Add(wave);
    }
  }

  void HandleNewWave(WaveManager waveManager)
  {
    if (currentWave) currentWave.SetFill(1);
    int index = waveManager.currentWave - 1;
    currentWave = waves[index];
  }

  void HandleVictory(MatchManager waveManager)
  {
    waves.ForEach(x => x.SetFill(1));
  }

  void HandleWaveProgress(float percentage)
  {
    currentWave.SetFill(percentage);
  }

  void OnDestroy()
  {

    WaveManager.OnNewSchema -= HandleNewSchema;
    WaveManager.OnNewWave -= HandleNewWave;
    WaveManager.OnWaveProgress -= HandleWaveProgress;
    MatchManager.OnVictory -= HandleVictory;
  }
}
