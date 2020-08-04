using Fralle.Core.Infrastructure;
using UnityEngine;

namespace Fralle.Core.Audio
{
  public class AudioSourcePool : PollingPool<AudioSource>
  {
    public AudioSourcePool(AudioSource prefab) : base(prefab)
    {
    }

    protected override bool IsActive(AudioSource component)
    {
      return component.isPlaying;
    }

    public AudioSource GetSource() => Get();

    public void PlayAtPoint(AudioClip clip, Vector3 point)
    {
      var source = Get();

      source.transform.position = point;
      source.clip = clip;
      source.Play();
    }
  }
}