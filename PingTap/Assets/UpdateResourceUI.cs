using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateResourceUI : MonoBehaviour
{
  [SerializeField] TextMeshProUGUI creditsText;

  [SerializeField] GameObject creditPrefab;

  [SerializeField] Vector2 randomizeDropPosition = new Vector2(25, 25);

  [SerializeField] LeanTweenType easeType = LeanTweenType.easeInBack;
  [SerializeField] float animationTime = 1f;
  [SerializeField] float staggerDelayTime = 0.25f;
  [SerializeField] float maxStaggerDelayTime = 1;

  InventoryController inventoryController;

  void Awake()
  {
    inventoryController = GetComponentInParent<InventoryController>();
    inventoryController.OnCreditsUpdate += HandleCreditsUpdate;
  }

  void HandleCreditsUpdate(int credits)
  {
    UpdateText(credits);
  }

  void UpdateText(int credits)
  {
    creditsText.text = $"Credits: {credits}";
  }
}
