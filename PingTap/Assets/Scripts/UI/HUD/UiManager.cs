using Fralle.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fralle.UI.HUD
{
  public class UiManager : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI gameStatusText;
    [FormerlySerializedAs("gameStatusUI")] [SerializeField] GameObject gameStatusUi;

    void Start()
    {
      gameStatusUi.SetActive(false);
      MatchManager.OnDefeat += HandleDefeat;
      MatchManager.OnVictory += HandleVictory;

    }

    void HandleDefeat(MatchManager matchManager)
    {
      gameStatusUi.SetActive(true);
      gameStatusText.text = "DEFEAT";
    }

    void HandleVictory(MatchManager matchManager)
    {
      gameStatusUi.SetActive(true);
      gameStatusText.text = "VICTORY";
    }

    void OnDestroy()
    {
      MatchManager.OnDefeat -= HandleDefeat;
      MatchManager.OnVictory -= HandleVictory;
    }
  }
}
