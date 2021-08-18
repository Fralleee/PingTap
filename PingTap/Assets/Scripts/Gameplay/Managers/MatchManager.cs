using Fralle.Core.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fralle.Gameplay
{
  public class MatchManager : MonoBehaviour
  {
    public static event Action OnMatchEnd = delegate { };
    public static event Action OnDefeat = delegate { };
    public static event Action OnVictory = delegate { };

    [Header("Audio")]
    [SerializeField] AudioEvent victorySound;
    [SerializeField] AudioEvent defeatSound;

    HeadQuarters headQuarters;
    bool isHqDead;

    public bool IsPlayingMatch => !SceneManager.GetSceneByName("Main Menu").isLoaded;

    public void Setup()
    {
      headQuarters = FindObjectOfType<HeadQuarters>();
      headQuarters.OnDeath += HeadQuarters_OnDeath;
    }

    public void MatchOver()
    {
      if (isHqDead)
      {
        Victory();
      }
      else
      {
        Defeat();
      }

      Managers.Instance.Camera.ActivateSceneCamera();
      OnMatchEnd();
    }

    void Victory()
    {
      Debug.Log("Victory");
      Managers.Instance.Audio.Play(victorySound);
      OnVictory();
    }

    void Defeat()
    {
      Debug.Log("Defeat");
      Managers.Instance.Audio.Play(defeatSound);
      OnDefeat();
    }

    void HeadQuarters_OnDeath(HeadQuarters obj)
    {
      isHqDead = true;
    }
  }
}
