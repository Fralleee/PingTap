using UnityEngine;

namespace Fralle.Gameplay
{
  public class TreasureSpawner : MonoBehaviour
  {
    [SerializeField] GameObject treasurePrefab = null;
    [SerializeField, Range(0, 1)] float spawnChance = 0f;

    Terrain terrain;

    void Awake()
    {
      terrain = FindObjectOfType<Terrain>();
    }

    public bool Spawn()
    {
      bool doSpawn = Random.value < spawnChance;
      if (!doSpawn) return false;

      var center = terrain.terrainData.bounds.center + terrain.transform.position;
      var extents = terrain.terrainData.bounds.extents;
      var position = center + new Vector3(Random.Range(-extents.x, extents.x), 0, Random.Range(-extents.z, extents.z));

      if (!Physics.Raycast(position + Vector3.up * 100f, Vector3.down, out var hit, 200.0f)) return false;

      Instantiate(treasurePrefab, hit.point, Quaternion.identity);
      return true;
    }
  }
}
