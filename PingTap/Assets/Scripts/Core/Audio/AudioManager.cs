using System.Collections;
using UnityEngine;

namespace Fralle.Core.Audio
{
  public class AudioManager : MonoBehaviour
  {
    AudioSourcePool audioSourcePool = null;
    [SerializeField] AudioSource source = null;

    void Awake()
    {
      audioSourcePool = new AudioSourcePool(source);
    }

    public void Play(AudioEvent audioEvent)
    {
      if (audioEvent.playCount > 1)
      {
        StartCoroutine(PlayIEnumerator(audioEvent));
      }
      else
      {
        audioEvent.Play(audioSourcePool.GetSource());
      }
    }

    IEnumerator PlayIEnumerator(AudioEvent audioEvent)
    {
      for (var i = 0; i < audioEvent.playCount; i++)
      {
        audioEvent.Play(audioSourcePool.GetSource());
        yield return new WaitForSeconds(audioEvent.playDelay);
      }
    }
  }
}