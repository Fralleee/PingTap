﻿using Fralle;
using Fralle.Attack;
using System.Globalization;
using UnityEngine;

public class FloatingNumbersController : MonoBehaviour
{
  [SerializeField] FloatingText prefab;
  new Camera camera;

  void Awake()
  {
    camera = Camera.main;
    Health.OnAnyDamage += AddFloatingNumber;
  }

  void AddFloatingNumber(Damage damage)
  {
    FloatingText floatingText = Instantiate(prefab, transform);

    string damageText = Mathf.Round(damage.damageAmount).ToString(CultureInfo.InvariantCulture);

    floatingText.Setup(damageText, damage.position, camera, damage.hitBoxType);
  }
}