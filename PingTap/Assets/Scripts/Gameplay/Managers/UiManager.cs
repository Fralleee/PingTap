using UnityEngine;

namespace Fralle.Gameplay
{
  public class UiManager : MonoBehaviour
  {
    [Header("Live Ui")]
    [SerializeField] GameObject prepareUi = null;
    [SerializeField] GameObject nexusUi = null;
    [SerializeField] GameObject waveCounterUi = null;
    [SerializeField] GameObject waveInfoUi = null;

    [Header("Result Ui")]
    [SerializeField] GameObject gameResultUi = null;

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
      Instantiate(nexusUi, liveUi.transform);

      Instantiate(gameResultUi, resultUi.transform);
    }


    void HandleGameEnd()
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