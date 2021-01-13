using Fralle.Gameplay;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fralle.UI.Menu
{
	public class MainMenu : MonoBehaviour
	{
		public static event Action<bool> OnMenuToggle = delegate { };

		[Header("Buttons")]
		[SerializeField] GameObject playButton = null;
		[SerializeField] GameObject resumeButton = null;
		[SerializeField] GameObject leaveButton = null;

		[Header("Other")]
		[SerializeField] GameObject background = null;
		[SerializeField] GameObject main = null;
		[SerializeField] GameObject levelSelect = null;
		[SerializeField] GameObject options = null;

		const string MainMenuScene = "Main menu";
		bool isOpen;

		public static void ToMainMenu()
		{
			SceneManager.LoadScene(MainMenuScene);
		}

		public void Play()
		{
			levelSelect.gameObject.SetActive(true);
			main.SetActive(false);
		}

		public void Resume()
		{
			ToggleMenu();
		}

		public void Options()
		{
			options.gameObject.SetActive(true);
			options.GetComponent<SubMenu>().inGame = StateManager.gameState == GameState.Playing;
			main.SetActive(false);
		}

		public void Quit()
		{
			Application.Quit();
		}

		public void ToggleMenu()
		{
			isOpen = !isOpen;

			gameObject.SetActive(isOpen);
			main.SetActive(isOpen);
			background.SetActive(isOpen);

			StateManager.SetGameState(isOpen ? GameState.MenuActive : GameState.Playing);
			OnMenuToggle(isOpen);
		}

		void OnEnable()
		{
			var inGame = StateManager.gameState == GameState.Playing;

			levelSelect.SetActive(false);
			options.SetActive(false);

			background.SetActive(!inGame);
			playButton.SetActive(!inGame);
			main.SetActive(!inGame);

			resumeButton.SetActive(inGame);
			leaveButton.SetActive(inGame);
		}
	}
}
