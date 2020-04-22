using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Fralle;
using TMPro;
using UnityEngine;

public class FloatingNumbersController : MonoBehaviour
{
  [SerializeField] FloatingText prefab;
  new Camera camera;

  void Awake()
  {
    camera = Camera.main;
    DamageController.OnAnyDamage += AddFloatingNumber;
  }

  void AddFloatingNumber(DamageData damageData)
  {
    FloatingText floatingText = Instantiate(prefab, transform);

    string damageText = Mathf.Round(damageData.damage).ToString(CultureInfo.InvariantCulture);

    floatingText.Setup(damageText, damageData.position, camera);
  }
}