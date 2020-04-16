﻿using System.Globalization;
using TMPro;
using UnityEngine;

namespace Fralle
{
  public class FloatingNumbers : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI numberPrefab;

    DamageController damageController;
    new Camera camera;
    new Renderer renderer;

    public void Initialize(DamageController damageController)
    {
      this.damageController = damageController;
      damageController.OnDamage += HandleDamage;

      camera = Camera.main;

      renderer = damageController.gameObject.GetComponentInChildren<Renderer>();
    }

    void HandleDamage(DamageData damageData, float damage, bool criticalHit)
    {
      TextMeshProUGUI instance = Instantiate(numberPrefab, transform);
      instance.text = Mathf.Round(damage).ToString(CultureInfo.InvariantCulture);
      Destroy(instance, 2f);
    }

    void OnDestroy()
    {
      damageController.OnDamage -= HandleDamage;
    }

    void LateUpdate()
    {
      if (damageController == null) return;
      if (renderer && !renderer.isVisible) return;

      transform.position = camera.WorldToScreenPoint(damageController.transform.position + Vector3.up * 2f);
    }

  }
}