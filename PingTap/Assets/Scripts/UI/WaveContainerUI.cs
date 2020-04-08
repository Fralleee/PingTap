using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveContainerUI : MonoBehaviour
{
  [SerializeField] RoundUI roundPrefab;

  [SerializeField] Sprite ground;
  [SerializeField] Sprite air;
  [SerializeField] Sprite invisible;
  [SerializeField] Sprite attacking;
  [SerializeField] Sprite boss;

  List<RoundUI> waves = new List<RoundUI>();

  void Awake()
  {
    WaveManager.OnNewSchema += HandleNewSchema;
    WaveManager.OnNewWave += HandleNewWave;
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
        round.SetImage(ground);
        break;
      case WaveType.Air:
        round.SetImage(air);
        break;
      case WaveType.Invisible:
        round.SetImage(invisible);
        break;
      case WaveType.Attacking:
        round.SetImage(attacking);
        break;
      case WaveType.Boss:
        round.SetImage(boss);
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof(waveType), waveType, null);
    }
  }

  void HandleNewWave(WaveManager waveManager)
  {
    int index = waveManager.schemaRound - 1;
    if (index > 0) waves[index - 1].Victory();
    waves[index].Activate();
  }

  void HandleVictory(MatchManager waveManager)
  {
    waves.ForEach(x => x.Victory());
  }
}
