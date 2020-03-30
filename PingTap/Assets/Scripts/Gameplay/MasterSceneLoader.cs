using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterSceneLoader : MonoBehaviour
{
  const string MASTERSCENESTR = "MasterScene";
  void Awake()
  {
    var masterScene = SceneManager.GetSceneByName(MASTERSCENESTR);
    if (masterScene == null) Debug.LogError("MasterScene not found");
    if (!masterScene.isLoaded) SceneManager.LoadScene(MASTERSCENESTR, LoadSceneMode.Additive);
  }
}
