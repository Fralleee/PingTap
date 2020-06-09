using Fralle.UI;
using UnityEngine;

namespace Fralle.Resource
{
  public class Loot : MonoBehaviour
  {
    static readonly int RendererColor = Shader.PropertyToID("_EmissionColor");

    [SerializeField] LootConfiguration lootConfiguration;
    [SerializeField] Renderer model;
    [SerializeField] Transform effectSpawnPoint;
    [SerializeField] float lifeTime = 60f;

    MaterialPropertyBlock propBlock;
    UiTweener uiTweener;
    bool pickedUp;
    int credits = 5;

    void Awake()
    {
      uiTweener = GetComponent<UiTweener>();
      propBlock = new MaterialPropertyBlock();
      Drop();
    }

    void Update()
    {
      lifeTime -= Time.deltaTime;
      if (lifeTime <= 0 || pickedUp)
      {
        DeSpawn();
      }
    }

    void Drop()
    {
      var quality = lootConfiguration.GetQuality();
      credits = lootConfiguration.GetQualityCredits(quality);
      propBlock.SetColor(RendererColor, lootConfiguration.GetQualityColor(quality));
      model.SetPropertyBlock(propBlock);

      var prefab = lootConfiguration.GetQualityPrefab(quality);
      var instance = Instantiate(prefab, transform);
      if (effectSpawnPoint) instance.transform.position = effectSpawnPoint.position;
    }

    void DeSpawn()
    {
      uiTweener.HandleTween();
      Destroy(gameObject, uiTweener.duration);
    }

    void OnTriggerEnter(Collider collider)
    {
      if (pickedUp) return;

      var inventoryController = collider.GetComponentInParent<InventoryController>();
      if (!inventoryController) return;

      inventoryController.Receive(credits);
      pickedUp = true;
    }
  }
}