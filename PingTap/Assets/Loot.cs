using Fralle.Core.Animation;
using Fralle.Resource;
using UnityEngine;

public class Loot : MonoBehaviour
{
  [SerializeField] int credits = 5;
  [SerializeField] float lifeTime = 60f;

  UiTweener uiTweener;

  bool pickedUp;

  void Awake()
  {
    uiTweener = GetComponent<UiTweener>();
  }

  void Update()
  {
    lifeTime -= Time.deltaTime;
    if (lifeTime <= 0 || pickedUp)
    {
      DeSpawn();
    }
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
