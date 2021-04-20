using UnityEngine;

namespace Fralle.UI.Menu
{
  public class SubMenu : MonoBehaviour
  {
    [SerializeField] GameObject parentMenu = null;

    public bool InGame;

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
      if (!InGame) parentMenu.gameObject.SetActive(true);
    }
  }
}