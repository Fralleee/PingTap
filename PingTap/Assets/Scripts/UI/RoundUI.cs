using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundUI : MonoBehaviour
{
  [SerializeField] Color defaultColor;
  [SerializeField] Color activeColor;
  [SerializeField] Color successColor;
  [SerializeField] Color defeatColor;

  Image image;

  void Awake()
  {
    image = GetComponent<Image>();
  }
  
  void ChangeColor(Color newColor)
  {
    image.color = newColor;
  }

  public void SetImage(Sprite sprite)
  {
    image.sprite = sprite;
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
