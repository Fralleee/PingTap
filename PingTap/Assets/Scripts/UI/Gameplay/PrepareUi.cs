using Fralle.Gameplay;
using TMPro;
using UnityEngine;

namespace Fralle.UI.HUD
{
	public class PrepareUi : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI prepareTimer = null;

		Canvas canvas;
		UiTweener uiTweener;

		float oldTimer;

		void Awake()
		{
			canvas = GetComponent<Canvas>();
			uiTweener = GetComponent<UiTweener>();

			EventManager.AddListener<GameStateChangeEvent>(OnGameStateChange);
		}

		void Update()
		{
			//if (MatchManager.Instance.gameState != GameState.Prepare) return;

			//if (MatchManager.Instance.prepareTimer >= 3) SetText(MatchManager.Instance.prepareTimer);
			//else if (MatchManager.Instance.prepareTimer < 3 && oldTimer >= 3) SetExplicitText("Ready");
			//else if (MatchManager.Instance.prepareTimer < 2 && oldTimer >= 2) SetExplicitText("Set");
			//else if (MatchManager.Instance.prepareTimer < 1 && oldTimer >= 1) SetExplicitText("Go");

			//prepareTimer.gameObject.SetActive(MatchManager.Instance.prepareTimer > 0);
			//oldTimer = MatchManager.Instance.prepareTimer;
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

		void OnGameStateChange(GameStateChangeEvent evt)
		{
			canvas.enabled = evt.NewGameState == GameState.Prepare;
		}

		void OnDestroy()
		{
			EventManager.RemoveListener<GameStateChangeEvent>(OnGameStateChange);
		}
	}
}
