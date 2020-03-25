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
    LevelManager.OnDefeat += HandleDefeat;
    LevelManager.OnVictory += HandleVictory;

  }
  void Dispose()
  {
    LevelManager.OnDefeat -= HandleDefeat;
    LevelManager.OnVictory -= HandleVictory;
  }

  void HandleDefeat(LevelManager levelManager)
  {
    gameStatusUI.SetActive(true);
    gameStatusText.text = "DEFEAT";
  }

  void HandleVictory(LevelManager levelManager)
  {
    gameStatusUI.SetActive(true);
    gameStatusText.text = "VICTORY";
  }
}
