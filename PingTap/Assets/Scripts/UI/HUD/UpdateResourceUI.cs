using Fralle.Resource;
using TMPro;
using UnityEngine;

namespace Fralle.UI.HUD
{
  public class UpdateResourceUi : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI creditsText = null;

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
}