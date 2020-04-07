using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

  // Probably extract stuff from here when there's too much happening

  [SerializeField] TextMeshProUGUI gameStatusText;
  [SerializeField] GameObject gameStatusUI;

  void Start()
  {
    gameStatusUI.SetActive(false);
    MatchManager.OnDefeat += HandleDefeat;
    MatchManager.OnVictory += HandleVictory;

  }

  void HandleDefeat(MatchManager matchManager)
  {
    gameStatusUI.SetActive(true);
    gameStatusText.text = "DEFEAT";
  }

  void HandleVictory(MatchManager matchManager)
  {
    gameStatusUI.SetActive(true);
    gameStatusText.text = "VICTORY";
  }

  void OnDestroy()
  {
    MatchManager.OnDefeat -= HandleDefeat;
    MatchManager.OnVictory -= HandleVictory;
  }
}
