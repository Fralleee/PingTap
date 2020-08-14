using CombatSystem.Combat.Damage;
using TMPro;
using UnityEngine;

public class TargetNameUI : MonoBehaviour
{
  TextMeshProUGUI nameText;

  void Awake()
  {
    nameText = GetComponent<TextMeshProUGUI>();
    var damageController = GetComponentInParent<DamageController>();
    nameText.text = damageController.name;
  }
}
