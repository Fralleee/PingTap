using UnityEngine;

public class DamageController : MonoBehaviour
{
  [SerializeField] float maxHealth = 100f;
  [SerializeField] float currentHealth;
  [SerializeField] int armor;

  public float damageMultiplier { get { return 1 - 0.06f * armor / (1 + 0.06f * armor); } }

  void Start()
  {
    if (currentHealth == 0) currentHealth = maxHealth;
  }

  void Die()
  {
    Destroy(gameObject);
  }

  void ChangeHealth(float delta)
  {
    currentHealth -= delta;
    currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    if (currentHealth == 0) Die();
  }

  public void TakeDamage(float rawDamage)
  {
    ChangeHealth(rawDamage * damageMultiplier);
  }
}
