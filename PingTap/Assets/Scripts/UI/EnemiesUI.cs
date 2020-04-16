using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Fralle;
using TMPro;
using UnityEngine;

public class EnemiesUI : MonoBehaviour
{
  [SerializeField] TextMeshProUGUI text;

  MatchManager matchManager;
  UITweener tweener;

  void Awake()
  {
    matchManager = GetComponentInParent<MatchManager>();
    tweener = GetComponentInParent<UITweener>();

    Enemy.OnAnyEnemyDeath += HandleEnemyDeath;
    MatchManager.OnNewRound += HandleNewRound;
  }

  void HandleNewRound(MatchManager matchManager)
  {
    SetText(matchManager.enemiesAlive);
  }

  void HandleEnemyDeath(Enemy enemy)
  {
    SetText(matchManager.enemiesAlive);
  }


  void SetText(int num)
  {
    text.text = $"Enemies alive: {Mathf.Round(num).ToString(CultureInfo.InvariantCulture)}";

    tweener.HandleTween();
  }
}
