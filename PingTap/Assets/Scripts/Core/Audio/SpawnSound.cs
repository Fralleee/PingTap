using Fralle.Core.Extensions;
using UnityEngine;

namespace Fralle.Core.Audio
{
  public class SpawnSound : MonoBehaviour
  {
    public GameObject prefabSound;

    public bool destroyWhenDone = true;
    public bool soundPrefabIsChild = false;

    [Range(0.01f, 10f)] public float pitchRandomMultiplier = 1f;

    void Start()
    {
      var sound = Instantiate(prefabSound, transform.position, Quaternion.identity);
      var source = sound.GetComponent<AudioSource>();

      if (soundPrefabIsChild) sound.transform.SetParent(transform);

      if (pitchRandomMultiplier.EqualsWithTolerance(1))
      {
        if (Random.value < .5) source.pitch *= Random.Range(1 / pitchRandomMultiplier, 1);
        else source.pitch *= Random.Range(1, pitchRandomMultiplier);
      }

      if (!destroyWhenDone) return;

      var life = source.clip.length / source.pitch;
      Destroy(sound, life);
    }
  }
}