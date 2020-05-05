using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateResourceUi : MonoBehaviour
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
    UpdateText(credits);
  }

  void UpdateText(int credits)
  {
    creditsText.text = $"Credits: {credits}";
  }
}
