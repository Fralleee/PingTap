using Fralle.Core.Gameplay;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fralle.Core
{
	[ExecuteInEditMode]
	public class EditorInitialisationLoader : MonoBehaviour
	{
		public GameSceneSO initializationScene;

		void OnEnable()
		{
			for (int i = 0; i < SceneManager.sceneCount; ++i)
			{
				Scene scene = SceneManager.GetSceneAt(i);
				if (scene.name == initializationScene.sceneName)
				{
					return;
				}
			}
#if UNITY_EDITOR
			StartCoroutine(LoadSceneDelayed());
#else
		SceneManager.LoadSceneAsync(initializationScene.name, LoadSceneMode.Additive);
#endif
		}

#if UNITY_EDITOR
		IEnumerator LoadSceneDelayed()
		{
			yield return new WaitForSecondsRealtime(0.1f);
			EditorSceneManager.OpenScene("Assets/Scenes/InitScene.unity", OpenSceneMode.Additive);
		}
#endif
	}
}
