using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveContainerUI : MonoBehaviour
{
  [SerializeField] RoundUI roundPrefab;

  [SerializeField] string ground;
  [SerializeField] string flying;
  [SerializeField] string invisible;
  [SerializeField] string attacking;
  [SerializeField] string boss;

  List<RoundUI> waves = new List<RoundUI>();
  RoundUI currentWave;

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

    Army army = waveManager.GetCurrentArmy;

    for (var i = 0; i < army.MaxRounds; i++)
    {
      RoundUI round = Instantiate(roundPrefab, transform);
      SetImage(round, army.waveDefinitions[i].enemy.waveType);
      waves.Add(round);
    }
  }

  void SetImage(RoundUI round, WaveType waveType)
  {
    switch (waveType)
    {
      case WaveType.Ground:
        round.SetText(ground);
        break;
      case WaveType.Flying:
        round.SetText(flying);
        break;
      case WaveType.Invisible:
        round.SetText(invisible);
        break;
      case WaveType.Attacking:
        round.SetText(attacking);
        break;
      case WaveType.Boss:
        round.SetText(boss);
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof(waveType), waveType, null);
    }
  }

  void HandleNewWave(WaveManager waveManager)
  {
    int index = waveManager.currentWave - 1;
    currentWave = waves[index];
    if (index > 0) waves[index - 1].Victory();
    currentWave.Activate();
  }

  void HandleVictory(MatchManager waveManager)
  {
    waves.ForEach(x => x.Victory());
  }

  void HandleWaveProgress(float percentage)
  {
    currentWave.UpdateFill(percentage);
  }
}
