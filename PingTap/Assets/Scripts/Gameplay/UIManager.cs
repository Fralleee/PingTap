using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fralle.Gameplay
{
  public class UiManager : MonoBehaviour
  {

    // Probably extract stuff from here when there's too much happening

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
