using Fralle.Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Fralle.UI.Menu
{
	public class PauseMenu : MonoBehaviour, PlayerControls.IMenuActions
	{
		const string MainMenuScene = "Main menu";

		[SerializeField] bool showMenuCanvas;
		[SerializeField] GameObject canvas;

		[Header("Other")]
		[SerializeField] GameObject root = null;
		[SerializeField] GameObject options = null;

		PlayerControls controls;

		bool isOpen;

		void Awake()
		{
			controls = new PlayerControls();
			controls.Menu.SetCallbacks(this);
			controls.Menu.Enable();
		}

		void ToggleMenu()
		{
			isOpen = !isOpen;
			PlayerInputController.ConfigureCursor(!isOpen);
			StateManager.SetGameState(isOpen ? GameState.PauseMenu : GameState.Playing);

			if (showMenuCanvas)
				canvas.SetActive(isOpen);
		}

		void OnEnable()
		{
			options.SetActive(false);
		}

		public void OnToggle(InputAction.CallbackContext context)
		{
			if (context.performed)
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
	}
}
