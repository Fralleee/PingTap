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
			options.GetComponent<SubMenu>().InGame = StateManager.GameState == GameState.Playing;
			root.SetActive(false);
		}

		public bool ToggleMenu()
		{
			isOpen = !isOpen;
			gameObject.SetActive(isOpen);
			PlayerInputController.ConfigureCursor(!isOpen);
			StateManager.SetGameState(isOpen ? GameState.PauseMenu : GameState.Playing);
			return isOpen;
		}

		void OnEnable()
		{
			options.SetActive(false);
		}
	}
}
