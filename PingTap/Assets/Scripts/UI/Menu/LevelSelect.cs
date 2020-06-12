using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fralle.UI.Menu
{
  public class LevelSelect : MonoBehaviour
  {
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject loadingCanvasPrefab = null;

    MainMenu menu;
    AsyncOperation operation;
    GameObject loadingCanvas;

    void Awake()
    {
      menu = FindObjectOfType<MainMenu>();

      loadingCanvas = Instantiate(loadingCanvasPrefab);
      loadingCanvas.SetActive(false);
    }

    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Escape)) OpenMainMenu();
    }

    public void OpenMainMenu()
    {
      menu.gameObject.SetActive(true);
      gameObject.SetActive(false);
    }

    public void Maul()
    {
      StartCoroutine(LoadScene("Maul"));
    }

    public void MaulV2()
    {
      StartCoroutine(LoadScene("Maul_v2"));
    }

    public void Timber()
    {
      StartCoroutine(LoadScene("Timber"));
    }

    IEnumerator LoadScene(string sceneName)
    {
      loadingCanvas.SetActive(true);
      gameObject.SetActive(false);
      operation = SceneManager.LoadSceneAsync(sceneName);

      while (!operation.isDone)
      {
        yield return null;
      }

      operation = null;
    }
  }
}