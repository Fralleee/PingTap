using Fralle.Gameplay;
using UnityEngine;

namespace Fralle.UI
{
  public class UiManager : MonoBehaviour
  {
    [Header("Live Ui")]
    [SerializeField] GameObject prepareUi;
    [SerializeField] GameObject nexusUi;
    [SerializeField] GameObject waveCounterUi;
    [SerializeField] GameObject waveInfoUi;

    [Header("Result Ui")]
    [SerializeField] GameObject gameResultUi;

    GameObject liveUi;
    GameObject resultUi;

    void Awake()
    {
      SetupTransforms();
      SetupUi();

      MatchManager.OnMatchEnd += HandleGameEnd;
    }

    void SetupTransforms()
    {
      liveUi = new GameObject("LiveUi");
      liveUi.transform.SetParent(transform);

      resultUi = new GameObject("ResultUi");
      resultUi.transform.SetParent(transform);
      resultUi.SetActive(false);
    }

    void SetupUi()
    {
      Instantiate(prepareUi, liveUi.transform);
      Instantiate(waveCounterUi, liveUi.transform);
      //Instantiate(waveInfoUi, liveUi.transform);
      Instantiate(nexusUi, liveUi.transform);

      Instantiate(gameResultUi, resultUi.transform);
    }


    void HandleGameEnd(MatchManager matchManager)
    {
      liveUi.SetActive(false);
      resultUi.SetActive(true);
    }

    void OnDestroy()
    {
      MatchManager.OnMatchEnd -= HandleGameEnd;
    }
  }
}