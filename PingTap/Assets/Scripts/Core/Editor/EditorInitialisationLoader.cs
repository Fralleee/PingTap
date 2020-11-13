using Fralle.Core.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fralle.Core
{
	public class EditorInitialisationLoader : MonoBehaviour
	{
#if UNITY_EDITOR
		public GameSceneSO initializationScene;

		void Start()
		{
			for (int i = 0; i < SceneManager.sceneCount; ++i)
			{
				Scene scene = SceneManager.GetSceneAt(i);
				if (scene.name == initializationScene.sceneName)
				{
					return;
				}
			}
			SceneManager.LoadSceneAsync(initializationScene.sceneName, LoadSceneMode.Additive);
		}
#endif
	}
}
