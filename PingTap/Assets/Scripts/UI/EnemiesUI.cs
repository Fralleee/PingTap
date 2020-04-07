using System;
using System.Collections;
using System.Collections.Generic;
using Fralle;
using TMPro;
using UnityEngine;

public class EnemiesUI : MonoBehaviour
{
  [SerializeField] TextMeshProUGUI timer;

  MatchManager matchManager;

  void Awake()
  {
    matchManager = GetComponentInParent<MatchManager>();
  }

  void Update()
  {
    SetText(matchManager.enemiesAlive);
  }

  void SetText(int num)
  {
    timer.text = $"Enemies alive: {Mathf.Round(num).ToString()}";
  }
}
