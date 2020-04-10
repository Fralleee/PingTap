using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundUI : MonoBehaviour
{
  [SerializeField] Color defaultColor;
  [SerializeField] Color activeColor;
  [SerializeField] Color successColor;
  [SerializeField] Color defeatColor;

  [SerializeField] Image iconBorder;
  [SerializeField] TextMeshProUGUI textUI;
  [SerializeField] Image fillBg;
  [SerializeField] Image fillImage;

  void ChangeColor(Color newColor)
  {
    iconBorder.color = newColor;
  }

  public void SetText(string text)
  {
    textUI.text = text;
  }

  public void DisableFill()
  {
    fillBg.enabled = false;
    fillImage.enabled = false;
  }

  public void Activate()
  {
    ChangeColor(activeColor);
  }

  public void Victory()
  {
    ChangeColor(successColor);
  }

  public void Defeat()
  {
    ChangeColor(defeatColor);
  }

  public void UpdateFill(float percent)
  {
    fillImage.fillAmount = percent;
  }

  void OnEnable()
  {
    ChangeColor(defaultColor);
  }
}
