using UnityEngine;

namespace Fralle.PingTap
{
  public class AIAttack : MonoBehaviour
  {
    public Weapon weapon;
    public Transform aim;
    public AIAttackPattern tap = new AIAttackPattern(new Vector2(0.4f, 0.6f), Vector2.one);
    public AIAttackPattern burst = new AIAttackPattern(new Vector2(0.5f, 0.9f), new Vector2(2, 4));
    public AIAttackPattern spray = new AIAttackPattern(Vector2.zero, new Vector2(10f, 15f));
    public Vector3 aimOffset { get; private set; }
    public float accuracy { get; private set; }

    Combatant combatant;
    AIAttackPattern currentAttackPattern;

    float lastAttack;
    int bulletsToFire;

    void Start()
    {
      if (combatant == null)
      {
        combatant = GetComponent<Combatant>();
        combatant.EquipWeapon(weapon);
      }

      currentAttackPattern = tap;
    }

    void SelectAttackPattern(float percentage)
    {
      if (percentage > 0.6f)
      {
        currentAttackPattern = tap;
      }
      else if (percentage > 0.35f)
      {
        currentAttackPattern = burst;
      }
      else
      {
        currentAttackPattern = spray;
      }
    }

    public void AdjustOffset(Vector3 newOffset)
    {
      aimOffset = newOffset;
    }

    public void AdjustAccuracy(float newAccuracy)
    {
      accuracy = newAccuracy;
    }

    public void Attack(Vector3 targetPosition, float attackRange)
    {
      if (bulletsToFire > 0 && combatant.EquippedWeapon.ActiveWeaponAction == Status.Ready)
      {
        aim.forward += new Vector3(0, Random.Range(-accuracy, accuracy), Random.Range(-accuracy, accuracy));
        combatant.PrimaryAction();
        bulletsToFire--;

        if (bulletsToFire == 0)
        {
          float nextAttack = Random.Range(currentAttackPattern.TimeBetweenAttacks.x, currentAttackPattern.TimeBetweenAttacks.y);
          lastAttack = Time.time + nextAttack;
        }
      }
      else if (bulletsToFire == 0)
        StartAttack(targetPosition, attackRange);
    }

    public void StartAttack(Vector3 targetPosition, float attackRange)
    {
      if (lastAttack > Time.time)
        return;

      float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
      SelectAttackPattern(distanceToTarget / attackRange);
      bulletsToFire = Random.Range((int)currentAttackPattern.BulletsPerAttack.x, (int)currentAttackPattern.BulletsPerAttack.y);
    }

    public void AimAt(Vector3 position)
    {
      aim.LookAt(position + aimOffset + Vector3.left * 0.1f, Vector3.up);
    }

  }
}
