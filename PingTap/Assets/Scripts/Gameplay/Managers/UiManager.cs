using System;
using UnityEngine;

namespace Fralle.Gameplay
{
  public class UiManager : MonoBehaviour
  {
    public event Action<bool> OnMenuToggle = delegate { };

    [Header("Settings")]
    //[SerializeField] bool allowPauseMenu = true;

    //[Header("References")]
    //[SerializeField] PauseMenu pauseMenu;

    [Header("Prefabs")]
    [SerializeField] GameObject prepareUiPrefab;
    [SerializeField] GameObject gameResultUiPrefab;

    GameObject prepareUi;
    GameObject gameResultUi;

    void Awake()
    {
      SetupUi();

      MatchManager.OnMatchEnd += HandleGameEnd;

      //if (!pauseMenu)
      //	FindObjectOfType<PauseMenu>();

      //pauseMenu.gameObject.SetActive(false);
    }

    void Update()
    {
      //if (!Input.GetKeyDown(KeyCode.Escape))
      //	return;

      //bool isOpen = pauseMenu.ToggleMenu(allowPauseMenu);
      //OnMenuToggle(isOpen);
    }

    void SetupUi()
    {
      prepareUi = SetupUiComponent(prepareUiPrefab);
      gameResultUi = SetupUiComponent(gameResultUiPrefab);
    }

    GameObject SetupUiComponent(GameObject prefab)
    {
      if (!prefab)
      {
        Debug.LogError($"UIManager prefab: {prefab.name} has not been set correctly.");
        return null;
      }


      GameObject instance = Instantiate(prefab, transform);
      instance.SetActive(false);
      return instance;
    }

    void HandleGameEnd()
    {
      prepareUi.SetActive(false);
      gameResultUi.SetActive(true);
    }

    void OnDestroy()
    {
      MatchManager.OnMatchEnd -= HandleGameEnd;
    }
  }
}
