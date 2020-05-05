using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveStatusUi : MonoBehaviour
{
  [SerializeField] Image foreGround;

  UITweener uiTweener;

  void Awake()
  {
    uiTweener = GetComponent<UITweener>();
  }

  public void SetFill(float percentage)
  {
    if (foreGround.fillAmount == 1) return;

    foreGround.fillAmount = percentage;
    if (percentage == 1) Animate();
  }

  void Animate()
  {
    uiTweener.HandleTween();
  }
}
