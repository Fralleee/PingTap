using UnityEngine;

namespace Fralle.PingTap
{
  public class AIAttack : MonoBehaviour
  {
    public Weapon Weapon;
    public Transform Aim;
    public AIAttackPattern Tap = new AIAttackPattern(new Vector2(0.4f, 0.6f), Vector2.one);
    public AIAttackPattern Burst = new AIAttackPattern(new Vector2(0.5f, 0.9f), new Vector2(2, 4));
    public AIAttackPattern Spray = new AIAttackPattern(Vector2.zero, new Vector2(10f, 15f));
    public Vector3 AimOffset { get; private set; }
    public float Accuracy { get; private set; }

    Combatant combatant;
    AIAttackPattern currentAttackPattern;

    float lastAttack;
    int bulletsToFire;

    void Start()
    {
      if (combatant == null)
      {
        combatant = GetComponent<Combatant>();
        combatant.EquipWeapon(Weapon);
      }

      currentAttackPattern = Tap;
    }

    void SelectAttackPattern(float percentage)
    {
      if (percentage > 0.6f)
      {
        currentAttackPattern = Tap;
      }
      else if (percentage > 0.35f)
      {
        currentAttackPattern = Burst;
      }
      else
      {
        currentAttackPattern = Spray;
      }
    }

    public void AdjustOffset(Vector3 newOffset)
    {
      AimOffset = newOffset;
    }

    public void AdjustAccuracy(float newAccuracy)
    {
      Accuracy = newAccuracy;
    }

    public void Attack(Vector3 targetPosition, float attackRange)
    {
      if (bulletsToFire <= 0 || combatant.equippedWeapon.ActiveWeaponAction != Status.Ready)
      {
        if (bulletsToFire == 0)
          StartAttack(targetPosition, attackRange);
      }
      else
      {
        Aim.forward += new Vector3(0, Random.Range(-Accuracy, Accuracy), Random.Range(-Accuracy, Accuracy));
        combatant.PrimaryAction();
        bulletsToFire--;

        if (bulletsToFire != 0)
          return;

        float nextAttack = Random.Range(currentAttackPattern.TimeBetweenAttacks.x, currentAttackPattern.TimeBetweenAttacks.y);
        lastAttack = Time.time + nextAttack;
      }
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
      Aim.LookAt(position + AimOffset + Vector3.left * 0.1f, Vector3.up);
    }

  }
}
