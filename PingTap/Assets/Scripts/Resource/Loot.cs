using UnityEngine;

namespace Fralle.Resource
{
  [RequireComponent(typeof(SphereCollider))]
  public class Loot : MonoBehaviour
  {
    static readonly int RendererColor = Shader.PropertyToID("_EmissionColor");

    new SphereCollider collider;

    [SerializeField] LootQuality.Type quality = LootQuality.Type.Poor;

    const float PickupRange = 3f;

    int credits;
    float lifeTime = 60f;
    bool pickedUp;

    public void Setup(int startCredits)
    {
      credits = startCredits;
    }

    void Awake()
    {
      collider = GetComponent<SphereCollider>();
      collider.radius = PickupRange;
      collider.isTrigger = true;

      Renderer rendererComponent = GetComponentInChildren<Renderer>();
      SetRendererColor(rendererComponent, quality);
    }

    void Update()
    {
      lifeTime -= Time.deltaTime;
      if (lifeTime <= 0)
        DeSpawn();
    }

    static void SetRendererColor(Renderer rendererP, LootQuality.Type qualityP)
    {
      if (!rendererP)
        return;

      MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
      rendererP.GetPropertyBlock(propBlock);
      propBlock.SetColor(RendererColor, LootQuality.GetQualityColor(qualityP));
      rendererP.SetPropertyBlock(propBlock);
    }

    void DeSpawn()
    {
      Destroy(gameObject, 1f);
    }

    void OnTriggerEnter(Component component)
    {
      if (pickedUp)
        return;

      InventoryController inventoryController = component.GetComponentInParent<InventoryController>();
      if (!inventoryController)
        return;

      inventoryController.Receive(credits);
      pickedUp = true;
      DeSpawn();
    }

  }
}
