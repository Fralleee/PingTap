using Fralle.UI;
using UnityEngine;

namespace Fralle.Resource
{
  [RequireComponent(typeof(SphereCollider))]
  public class Loot : MonoBehaviour
  {
    static readonly int RendererColor = Shader.PropertyToID("_EmissionColor");

    UiTweener uiTweener;
    SphereCollider collider;

    [SerializeField] LootQuality.Type quality;

    const float PickupRange = 3f;

    int credits;
    float lifeTime = 60f;
    bool pickedUp;

    public void Setup(int credits)
    {
      this.credits = credits;
    }

    void Awake()
    {
      uiTweener = GetComponent<UiTweener>();

      collider = GetComponent<SphereCollider>();
      collider.radius = PickupRange;
      collider.isTrigger = true;

      var renderer = GetComponentInChildren<Renderer>();
      SetRendererColor(renderer, quality);
    }

    void Update()
    {
      lifeTime -= Time.deltaTime;
      if (lifeTime <= 0) DeSpawn();
    }

    void SetRendererColor(Renderer renderer, LootQuality.Type quality)
    {
      if (!renderer) return;

      var propBlock = new MaterialPropertyBlock();
      renderer.GetPropertyBlock(propBlock);
      propBlock.SetColor(RendererColor, LootQuality.GetQualityColor(quality));
      renderer.SetPropertyBlock(propBlock);
    }

    void DeSpawn()
    {
      uiTweener.HandleTween();
      Destroy(gameObject, uiTweener.duration);
    }

    void OnTriggerEnter(Component component)
    {
      if (pickedUp) return;

      var inventoryController = component.GetComponentInParent<InventoryController>();
      if (!inventoryController) return;

      inventoryController.Receive(credits);
      pickedUp = true;
      DeSpawn();
    }

  }
}