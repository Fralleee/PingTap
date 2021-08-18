using Fralle.Resource;
using UnityEngine;

namespace Fralle.Gameplay
{
  public class Treasure : MonoBehaviour
  {
    [Header("Drops")]
    [SerializeField] LootTable lootTable = null;

    public float DestroyAfterDrop = 5f;

    void Awake()
    {
      Drop();
    }

    void Drop()
    {
      if (!lootTable)
        return;
      lootTable.DropLoot(transform.position);
      Destroy(gameObject, DestroyAfterDrop);
    }
  }
}
