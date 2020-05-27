using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fralle.UI.Menu
{
  public class MainMenu : MonoBehaviour
  {
    public static event Action<bool> OnMenuToggle = delegate { };

    [Header("Buttons")] [SerializeField] GameObject playButton;
    [SerializeField] GameObject resumeButton;
    [SerializeField] GameObject leaveButton;

    [Header("Other")] [SerializeField] GameObject background;
    [SerializeField] GameObject main;
    [SerializeField] GameObject levelSelect;
    [SerializeField] GameObject options;

    const string MainMenuScene = "Main menu";
    bool isOpen;
    bool inGame;

    void Awake()
    {
      var currentScene = SceneManager.GetActiveScene();
      inGame = currentScene.name != MainMenuScene;

      levelSelect.SetActive(false);
      options.SetActive(false);

      background.SetActive(!inGame);
      playButton.SetActive(!inGame);
      main.SetActive(!inGame);

      resumeButton.SetActive(inGame);
      leaveButton.SetActive(inGame);
    }

    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Escape) && inGame)
      {
        ToggleMenu();
      }
    }

    public static void ToMainMenu()
    {
      SceneManager.LoadScene(MainMenuScene);
    }

    public void Play()
    {
      levelSelect.gameObject.SetActive(true);
      main.SetActive(false);
    }

    public void Resume()
    {
      ToggleMenu();
    }

    public void Options()
    {
      options.gameObject.SetActive(true);
      options.GetComponent<SubMenu>().inGame = inGame;
      main.SetActive(false);
    }

    public void Quit()
    {
      Application.Quit();
    }

    void ToggleMenu()
    {
      isOpen = !isOpen;
      main.SetActive(isOpen);
      background.SetActive(isOpen);

      OnMenuToggle(isOpen);
    }
  }
}