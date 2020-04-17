using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmyUI : MonoBehaviour
{
  [SerializeField] Color defaultColor;
  [SerializeField] Color activeColor;
  [SerializeField] Color successColor;
  [SerializeField] Color defeatColor;

  [SerializeField] Image icon;

  void ChangeColor(Color newColor)
  {
    icon.color = newColor;
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

  void OnEnable()
  {
    ChangeColor(defaultColor);
  }
}
