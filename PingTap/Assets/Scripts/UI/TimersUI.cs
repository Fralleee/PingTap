using System;
using System.Collections;
using System.Collections.Generic;
using Fralle;
using TMPro;
using UnityEngine;

public class TimersUI : MonoBehaviour
{
  [SerializeField] TextMeshProUGUI timer;

  readonly Dictionary<GameState, Func<MatchManager, float>> gameStateMapper = new Dictionary<GameState, Func<MatchManager, float>>()
  {
    { GameState.Prepare, (matchManager) => matchManager.prepareTimer },
    { GameState.Live, (matchManager) => matchManager.roundTimer },
    { GameState.End, (matchManager) => 0f }
  };

  MatchManager matchManager;

  void Awake()
  {
    matchManager = GetComponentInParent<MatchManager>();
  }

  void Update()
  {
    float time = gameStateMapper[matchManager.gameState](matchManager);
    SetText(time);
  }

  void SetText(float num)
  {
    timer.text = Mathf.Round(num).ToString();
  }
}
