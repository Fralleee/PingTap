using Fralle.UI.Menu;
using QFSW.QC;
using UnityEngine;

namespace Fralle.Gameplay
{
	public class UiManager : MonoBehaviour
	{
		[Header("References")]
		[SerializeField] MainMenu mainMenu;

		[Header("Prefabs")]
		[SerializeField] GameObject prepareUiPrefab;
		[SerializeField] GameObject gameResultUiPrefab;

		GameObject prepareUi;
		GameObject gameResultUi;

		void Awake()
		{
			SetupUi();

			MatchManager.OnMatchEnd += HandleGameEnd;
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape) && !QuantumConsole.Instance.IsActive)
			{
				mainMenu.ToggleMenu();
			}
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
