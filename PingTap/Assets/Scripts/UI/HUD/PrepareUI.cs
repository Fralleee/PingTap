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
    UiTweener uiTweener;

    float oldTimer;

    void Awake()
    {
      canvas = GetComponent<Canvas>();
      uiTweener = GetComponent<UiTweener>();

      matchManager = GetComponentInParent<MatchManager>();
      MatchManager.OnNewState += HandleNewState;
    }

    void Update()
    {
      if (matchManager.gameState != GameState.Prepare) return;

      if (matchManager.prepareTimer >= 3) SetText(matchManager.prepareTimer);
      else if (matchManager.prepareTimer < 3 && oldTimer >= 3) SetExplicitText("Ready");
      else if (matchManager.prepareTimer < 2 && oldTimer >= 2) SetExplicitText("Set");
      else if (matchManager.prepareTimer < 1 && oldTimer >= 1) SetExplicitText("Go");

      prepareTimer.gameObject.SetActive(matchManager.prepareTimer > 0);
      oldTimer = matchManager.prepareTimer;
    }

    void SetExplicitText(string text)
    {
      prepareTimer.text = text;
      uiTweener.HandleTween();
    }

    void SetText(float num)
    {
      var minutes = Mathf.Floor(num / 60).ToString("00");
      var seconds = Mathf.Floor(num % 60).ToString("00");
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