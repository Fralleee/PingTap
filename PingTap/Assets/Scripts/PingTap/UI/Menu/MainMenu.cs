using Fralle.Gameplay;
using UnityEngine;

namespace Fralle.UI.Menu
{
	public class MainMenu : MonoBehaviour
	{
		[Header("Other")]
		[SerializeField] GameObject root;
		[SerializeField] GameObject levelSelect;
		[SerializeField] GameObject options;

		public void Play()
		{
			levelSelect.gameObject.SetActive(true);
			root.SetActive(false);
		}

		public void Options()
		{
			options.gameObject.SetActive(true);
			options.GetComponent<SubMenu>().inGame = StateManager.gameState == GameState.Playing;
			root.SetActive(false);
		}

		public void Quit()
		{
			Application.Quit();
		}

		void OnEnable()
		{
			StateManager.SetGameState(GameState.MainMenu);
		}
	}
}
