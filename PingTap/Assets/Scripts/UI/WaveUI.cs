using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
  [SerializeField] Color defaultColor;
  [SerializeField] Color activeColor;
  [SerializeField] Color successColor;
  [SerializeField] Color defeatColor;

  Image[] images;

  void Awake()
  {
    images = GetComponentsInChildren<Image>();
  }

  void ChangeColor(Color newColor)
  {
    foreach (Image image in images)
    {
      image.color = newColor;
    }
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
