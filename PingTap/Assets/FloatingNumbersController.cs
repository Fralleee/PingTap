using System.Collections;
using System.Collections.Generic;
using Fralle;
using UnityEngine;

public class FloatingNumbersController : MonoBehaviour
{
  [SerializeField] FloatingNumbers floatingNumbersPrefab;

  Dictionary<DamageController, FloatingNumbers> floatingNumbers = new Dictionary<DamageController, FloatingNumbers>();

  void Awake()
  {
    DamageController.OnHealthBarAdded += AddFloatingNumbers;
    DamageController.OnHealthBarRemoved += RemoveFloatingNumbers;
  }

  void AddFloatingNumbers(DamageController damageController)
  {
    if (floatingNumbers.ContainsKey(damageController)) return;

    FloatingNumbers floatingNumbersInstance = Instantiate(floatingNumbersPrefab, transform);
    floatingNumbersInstance.Initialize(damageController);
  }

  void RemoveFloatingNumbers(DamageController damageController)
  {
    if (!floatingNumbers.ContainsKey(damageController)) return;

    Destroy(floatingNumbers[damageController].gameObject);
    floatingNumbers.Remove(damageController);
  }
}