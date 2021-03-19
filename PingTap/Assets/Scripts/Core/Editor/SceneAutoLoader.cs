#if (UNITY_EDITOR)
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fralle.Core
{
	/// <summary>
	/// Scene auto loader.
	/// </summary>
	/// <description>
	/// This class adds a File > Scene Autoload menu containing options to select
	/// a "master scene" enable it to be auto-loaded when the user presses play
	/// in the editor. When enabled, the selected scene will be loaded on play,
	/// then the original scene will be reloaded on stop.
	///
	/// Based on an idea on this thread:
	/// http://forum.unity3d.com/threads/157502-Executing-first-scene-in-build-settings-when-pressing-play-button-in-editor
	/// </description>
	[InitializeOnLoad]
	static class SceneAutoLoader
	{
		// Static constructor binds a playmode-changed callback.
		// [InitializeOnLoad] above makes sure this gets executed.
		static SceneAutoLoader()
		{
			EditorApplication.playModeStateChanged += OnPlayModeChanged;
		}

		// Menu items to select the "master" scene and control whether or not to load it.
		[MenuItem("File/Scene Autoload/Select Master Scene...")]
		static void SelectMasterScene()
		{
			string masterScene = EditorUtility.OpenFilePanel("Select Master Scene", Application.dataPath, "unity");
			masterScene = masterScene.Replace(Application.dataPath, "Assets");  //project relative instead of absolute path
			if (!string.IsNullOrEmpty(masterScene))
			{
				MasterScene = masterScene;
				LoadMasterOnPlay = true;
			}
		}

		[MenuItem("File/Scene Autoload/Load Master On Play", true)]
		static bool ShowLoadMasterOnPlay()
		{
			return !LoadMasterOnPlay;
		}
		[MenuItem("File/Scene Autoload/Load Master On Play")]
		static void EnableLoadMasterOnPlay()
		{
			LoadMasterOnPlay = true;
		}

		[MenuItem("File/Scene Autoload/Don't Load Master On Play", true)]
		static bool ShowDontLoadMasterOnPlay()
		{
			return LoadMasterOnPlay;
		}
		[MenuItem("File/Scene Autoload/Don't Load Master On Play")]
		static void DisableLoadMasterOnPlay()
		{
			LoadMasterOnPlay = false;
		}

		static void OnPlayModeChanged(PlayModeStateChange state)
		{
			if (!LoadMasterOnPlay || SceneManager.GetActiveScene().path == MasterScene)
			{
				return;
			}

			// Started Playing
			if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
			{
				PreviousScene = SceneManager.GetActiveScene().path;
				KeepMasterSceneOpen = MasterSceneIsOpen;

				if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() && !KeepMasterSceneOpen)
				{
					try
					{
						EditorSceneManager.OpenScene(MasterScene, OpenSceneMode.Additive);
					}
					catch
					{
						Debug.LogError(string.Format("error: scene not found: {0}", MasterScene));
						EditorApplication.isPlaying = false;

					}
				}
				else
				{
					EditorApplication.isPlaying = false;
				}
			}

			// Stopped Playing
			if (!EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode && !KeepMasterSceneOpen)
			{
				try
				{
					EditorSceneManager.CloseScene(SceneManager.GetSceneByPath(MasterScene), true);
				}
				catch
				{
					Debug.LogError(string.Format("error: scene not found: {0}", PreviousScene));
				}
			}
		}

		// Properties are remembered as editor preferences.
		const string cEditorPrefLoadMasterOnPlay = "Fralle.SceneAutoLoader.LoadMasterOnPlay";
		const string cEditorPrefMasterScene = "Fralle.SceneAutoLoader.MasterScene";
		const string cEditorPrefPreviousScene = "Fralle.SceneAutoLoader.PreviousScene";

		static bool LoadMasterOnPlay
		{
			get { return EditorPrefs.GetBool(cEditorPrefLoadMasterOnPlay, false); }
			set { EditorPrefs.SetBool(cEditorPrefLoadMasterOnPlay, value); }
		}

		static bool MasterSceneIsOpen
		{
			get
			{
				for (int i = 0; i < SceneManager.sceneCount; i++)
				{
					if (SceneManager.GetSceneAt(i).path == MasterScene)
						return true;
				}
				return false;
			}
		}
		static bool KeepMasterSceneOpen;
		static string MasterScene
		{
			get { return EditorPrefs.GetString(cEditorPrefMasterScene, "Master.unity"); }
			set { EditorPrefs.SetString(cEditorPrefMasterScene, value); }
		}

		static string PreviousScene
		{
			get { return EditorPrefs.GetString(cEditorPrefPreviousScene, SceneManager.GetActiveScene().path); }
			set { EditorPrefs.SetString(cEditorPrefPreviousScene, value); }
		}
	}
}

#endif
