﻿using Fralle.UI;
using UnityEngine;

namespace Fralle.Resource
{
  [RequireComponent(typeof(SphereCollider))]
  public class Loot : MonoBehaviour
  {
    static readonly int RendererColor = Shader.PropertyToID("_EmissionColor");

    UiTweener uiTweener;
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
      uiTweener = GetComponent<UiTweener>();

      collider = GetComponent<SphereCollider>();
      collider.radius = PickupRange;
      collider.isTrigger = true;

      var rendererComponent = GetComponentInChildren<Renderer>();
      SetRendererColor(rendererComponent, quality);
    }

    void Update()
    {
      lifeTime -= Time.deltaTime;
      if (lifeTime <= 0) DeSpawn();
    }

    static void SetRendererColor(Renderer rendererP, LootQuality.Type qualityP)
    {
      if (!rendererP) return;

      var propBlock = new MaterialPropertyBlock();
      rendererP.GetPropertyBlock(propBlock);
      propBlock.SetColor(RendererColor, LootQuality.GetQualityColor(qualityP));
      rendererP.SetPropertyBlock(propBlock);
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