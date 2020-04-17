using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyContainerUI : MonoBehaviour
{
  [SerializeField] ArmyUI armyPrefab;

  List<ArmyUI> armies = new List<ArmyUI>();

  void Awake()
  {
    WaveManager.OnNewSchema += HandleNewSchema;
    MatchManager.OnVictory += HandleVictory;
  }

  void HandleNewSchema(WaveManager waveManager)
  {
    if (armies.Count == 0) Initialize(waveManager);

    int index = waveManager.currentArmy;
    if (index > 0) armies[index - 1].Victory();
    armies[index].Activate();
  }

  void HandleVictory(MatchManager waveManager)
  {
    armies.ForEach(x => x.Victory());
  }

  void Initialize(WaveManager waveManager)
  {
    for (var i = 0; i < waveManager.armies.Length; i++)
    {
      ArmyUI army = Instantiate(armyPrefab, transform);
      armies.Add(army);
    }
  }
}
