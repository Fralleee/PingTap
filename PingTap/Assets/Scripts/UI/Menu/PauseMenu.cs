using Fralle.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fralle.UI.Menu
{
	public class PauseMenu : MonoBehaviour
	{
		[Header("Other")]
		[SerializeField] GameObject root = null;
		[SerializeField] GameObject options = null;

		const string MainMenuScene = "Main menu";
		bool isOpen;

		public void Resume()
		{
			ToggleMenu();
		}

		public static void ToMainMenu()
		{
			SceneManager.LoadScene(MainMenuScene);
		}

		public void Options()
		{
			options.gameObject.SetActive(true);
			options.GetComponent<SubMenu>().inGame = StateManager.gameState == GameState.Playing;
			root.SetActive(false);
		}

		public void ToggleMenu()
		{
			isOpen = !isOpen;
			gameObject.SetActive(isOpen);
			CameraManager.ConfigureCursor(!isOpen);
			StateManager.SetGameState(isOpen ? GameState.PauseMenu : GameState.Playing);
		}

		void OnEnable()
		{
			options.SetActive(false);
		}
	}
}
