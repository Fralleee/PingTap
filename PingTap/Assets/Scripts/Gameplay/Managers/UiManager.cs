using UnityEngine;

namespace Fralle.Gameplay
{
	public class UiManager : MonoBehaviour
	{
		[Header("Prefabs")]
		[SerializeField] GameObject prepareUiPrefab = null;
		[SerializeField] GameObject gameResultUiPrefab = null;

		GameObject prepareUi;
		GameObject gameResultUi;

		void Awake()
		{
			SetupUi();

			MatchManager.OnMatchEnd += HandleGameEnd;
		}

		void SetupUi()
		{
			prepareUi = SetupUiComponent(prepareUiPrefab);
			gameResultUi = SetupUiComponent(gameResultUiPrefab);
		}

		GameObject SetupUiComponent(GameObject prefab)
		{
			if (!prefab)
			{
				Debug.LogError($"UIManager prefab: {prefab.name} has not been set correctly.");
				return null;
			}


			var instance = Instantiate(prefab, transform);
			instance.SetActive(false);
			return instance;
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
