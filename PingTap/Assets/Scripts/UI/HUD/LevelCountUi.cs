﻿using Fralle.Gameplay;
using TMPro;
using UnityEngine;

namespace Fralle.UI.HUD
{
  public class LevelCountUi : MonoBehaviour
  {
    TextMeshProUGUI text;

    void Awake()
    {
      text = GetComponent<TextMeshProUGUI>();
      WaveManager.OnNewSchema += SetText;
    }

    void SetText(WaveManager waveManager)
    {
      text.text = $"Level {waveManager.currentArmy + 1}";
    }
  }
}