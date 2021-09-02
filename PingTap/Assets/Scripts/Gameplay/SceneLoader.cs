using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fralle.Gameplay
{
  public class SceneLoader : MonoBehaviour
  {
    public string menuScene;
    public string masterScene;

    void Awake()
    {
      Scene menu = SceneManager.GetSceneByName(menuScene);
      if (!menu.isLoaded)
        SceneManager.LoadScene(menuScene, LoadSceneMode.Additive);

      Scene master = SceneManager.GetSceneByName(masterScene);
      if (!master.isLoaded)
        SceneManager.LoadScene(masterScene, LoadSceneMode.Additive);
    }
  }
}
