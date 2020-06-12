using Fralle.Gameplay;
using Fralle.Player;
using Fralle.UI;
using Fralle.UI.Menu;
using TMPro;
using UnityEngine;

public class GameResultUi : MonoBehaviour
{
  [SerializeField] TextMeshProUGUI statusText = null;
  [SerializeField] GameObject statusScreen = null;
  [SerializeField] GameObject scoreScreen = null;

  PlayerStats stats;

  void Awake()
  {
    statusText.gameObject.SetActive(false);
    statusScreen.SetActive(false);
    scoreScreen.SetActive(false);

    MatchManager.OnDefeat += HandleDefeat;
    MatchManager.OnVictory += HandleVictory;
  }

  public void ScoreScreen()
  {
    statusScreen.SetActive(false);
    scoreScreen.SetActive(true);
    var scoreScreenComponent = scoreScreen.GetComponent<ScoreScreen>();
    scoreScreenComponent.InitPlayerStats(stats);
  }

  public void BackToMenu()
  {
    MainMenu.ToMainMenu();
  }

  void HandleDefeat(PlayerStats statsP)
  {
    stats = statsP;
    statusText.gameObject.SetActive(true);
    statusScreen.SetActive(true);
    statusText.text = "DEFEAT";
  }

  void HandleVictory(PlayerStats statsP)
  {
    stats = statsP;
    statusText.gameObject.SetActive(true);
    statusScreen.SetActive(true);
    statusText.text = "VICTORY";
  }

  void OnDestroy()
  {
    MatchManager.OnDefeat -= HandleDefeat;
    MatchManager.OnVictory -= HandleVictory;
  }
}
