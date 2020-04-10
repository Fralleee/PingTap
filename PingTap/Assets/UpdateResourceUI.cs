using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateResourceUI : MonoBehaviour
{
  [SerializeField] TextMeshProUGUI creditsText;

  InventoryController inventoryController;

  void Awake()
  {
    inventoryController = GetComponentInParent<InventoryController>();

    inventoryController.OnCreditsUpdate += HandleCreditsUpdate;
  }

  void HandleCreditsUpdate(int credits)
  {
    creditsText.text = $"Credits: {credits}";
  }
}
