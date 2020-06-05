using System.Collections;
using UnityEngine;

namespace Fralle.Core.Audio
{
  public class SpawnSounds : MonoBehaviour
  {
    public GameObject prefabSound;

    public int spawnCount = 1;

    public float spawnDelay = 1f;

    public bool destroyWhenDone = true;

    [Range(0.01f, 10f)] public float pitchRandomMultiplier = 1f;

    public void Spawn()
    {
      if (spawnDelay > 0) StartCoroutine(SpawnSingle(spawnDelay));
      else SpawnSingle();
    }

    IEnumerator SpawnSingle(float time)
    {
      for (var i = 0; i < spawnCount; i++)
      {
        SpawnSingle();
        yield return new WaitForSeconds(time);
      }
    }

    void SpawnSingle()
    {
      var sound = Instantiate(prefabSound, transform.position, Quaternion.identity);
      var source = sound.GetComponent<AudioSource>();

      if (pitchRandomMultiplier != 1)
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