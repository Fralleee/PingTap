using UnityEngine;

namespace Fralle.UI.Menu
{
  public class SubMenu : MonoBehaviour
  {
    [SerializeField] GameObject parentMenu;

    public bool inGame;

    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Escape)) ParentMenu();
    }

    public void Back()
    {
      gameObject.SetActive(false);
      parentMenu.gameObject.SetActive(true);
    }

    public void ParentMenu()
    {
      gameObject.SetActive(false);
      if (!inGame) parentMenu.gameObject.SetActive(true);
    }
  }
}