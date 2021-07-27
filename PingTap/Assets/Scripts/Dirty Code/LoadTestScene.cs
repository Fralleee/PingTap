using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTestScene : MonoBehaviour
{
	void Start()
	{
		SceneManager.LoadScene("MasterScene", LoadSceneMode.Additive);
		SceneManager.LoadSceneAsync("Combat AI Testing", LoadSceneMode.Additive);
	}
}
