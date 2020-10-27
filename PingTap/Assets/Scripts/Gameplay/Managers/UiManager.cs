using UnityEngine;

namespace Fralle.Gameplay
{
	public class UiManager : MonoBehaviour
	{
		[Header("Prefabs")]
		[SerializeField] GameObject prepareUiPrefab = null;
		[SerializeField] GameObject gameResultUiPrefab = null;

		[Header("Container")]
		[SerializeField] Transform uiTransform;

		GameObject prepareUi;
		GameObject gameResultUi;


		void Awake()
		{
			SetupTransforms();
			SetupUi();

			MatchManager.OnMatchEnd += HandleGameEnd;
		}

		void SetupTransforms()
		{
			if (uiTransform == null)
			{
				uiTransform = transform.Find("Ui");
			}
		}

		void SetupUi()
		{
			prepareUi = Instantiate(prepareUiPrefab, uiTransform);
			gameResultUi = Instantiate(gameResultUiPrefab, uiTransform);
			gameResultUi.SetActive(false);

		}

		void HandleGameEnd()
		{
			prepareUi.SetActive(false);
			gameResultUi.SetActive(true);
		}

		void OnDestroy()
		{
			MatchManager.OnMatchEnd -= HandleGameEnd;
		}
	}
}
