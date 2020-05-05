using Fralle.Gameplay;
using TMPro;
using UnityEngine;

namespace Fralle.UI.HUD
{
  public class PrepareUi : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI prepareTimer;

    MatchManager matchManager;
    Canvas canvas;

    void Awake()
    {
      canvas = GetComponent<Canvas>();

      matchManager = GetComponentInParent<MatchManager>();
      MatchManager.OnNewState += HandleNewState;
    }

    void Update()
    {
      if (matchManager.gameState == GameState.Prepare) SetText(matchManager.prepareTimer);
    }

    void SetText(float num)
    {
      var minutes = Mathf.Floor(num / 60).ToString("00");
      var seconds = (num % 60).ToString("00");
      prepareTimer.text = $"{minutes}:{seconds}";
    }

    void HandleNewState(GameState newState)
    {
      canvas.enabled = newState == GameState.Prepare;
    }

    void OnDestroy()
    {
      MatchManager.OnNewState -= HandleNewState;
    }
  }
}