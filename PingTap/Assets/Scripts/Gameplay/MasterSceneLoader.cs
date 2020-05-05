using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fralle.Gameplay
{
  public class MasterSceneLoader : MonoBehaviour
  {
    const string Masterscene = "MasterScene";

    void Awake()
    {
      var masterScene = SceneManager.GetSceneByName(Masterscene);
      if (!masterScene.isLoaded) SceneManager.LoadScene(Masterscene, LoadSceneMode.Additive);
    }
  }
}