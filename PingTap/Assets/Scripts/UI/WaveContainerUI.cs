using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveContainerUI : MonoBehaviour
{
  [SerializeField] WaveUI wavePrefab;

  List<WaveUI> waves = new List<WaveUI>();

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
    
    for (var i = 0; i < waveManager.maxRounds; i++)
    {
      WaveUI wave = Instantiate(wavePrefab, transform);
      waves.Add(wave);
    }
  }

  void HandleNewWave(WaveManager waveManager)
  {
    int index = waveManager.schemaRound - 1;
    if(index > 0) waves[index - 1].Victory();
    waves[index].Activate();
  }

  void HandleVictory(MatchManager waveManager)
  {
    waves.ForEach(x => x.Victory());
  }
}
