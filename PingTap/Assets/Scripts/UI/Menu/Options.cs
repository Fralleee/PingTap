using UnityEngine;

namespace Fralle.UI.Menu
{
  public class Options : MonoBehaviour
  {
    [SerializeField] GameObject backButton;

    MainMenu menu;

    void Awake()
    {
      menu = FindObjectOfType<MainMenu>();
    }

    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Escape))
        OpenMainMenu();
    }

    public void OpenMainMenu()
    {
      menu.gameObject.SetActive(true);
      gameObject.SetActive(false);
    }
  }
}