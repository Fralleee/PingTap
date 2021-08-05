using Fralle.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fralle.Gameplay
{
	public class SceneLoader : MonoBehaviour
	{
		[Scene] public string MenuScene;
		[Scene] public string MasterScene;

		void Awake()
		{
			Scene menu = SceneManager.GetSceneByName(MenuScene);
			if (!menu.isLoaded)
				SceneManager.LoadScene(MenuScene, LoadSceneMode.Additive);

			Scene master = SceneManager.GetSceneByName(MasterScene);
			if (!master.isLoaded)
				SceneManager.LoadScene(MasterScene, LoadSceneMode.Additive);
		}
	}
}
