using Fralle.Core.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fralle.Gameplay
{
  public class SceneLoader : MonoBehaviour
  {
    [Scene] public string menuScene;
    [Scene] public string masterScene;

    void Awake()
    {
      var menu = SceneManager.GetSceneByName(menuScene);
      if (!menu.isLoaded) SceneManager.LoadScene(menuScene, LoadSceneMode.Additive);

      var master = SceneManager.GetSceneByName(masterScene);
      if (!master.isLoaded) SceneManager.LoadScene(masterScene, LoadSceneMode.Additive);
    }
  }
}