using Fralle.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.PingTap
{
  public static class DamageHelper
  {
    public static DamageEffect[] SetupDamageEffects(DamageEffect[] damageEffects, Combatant combatant, float damageAmount)
    {
      for (int i = 0; i < damageEffects.Length; i++)
      {
        damageEffects[i] = damageEffects[i].Setup(combatant, damageAmount);
      }

      return damageEffects;
    }

    public static DamageData RaycastHit(RaycastAttack raycastAttack, RaycastHit hit)
    {
      AddForce(raycastAttack, hit);

      bool hitboxHit = raycastAttack.Combatant.teamController.Hitboxes.IsInLayerMask(hit.collider.gameObject.layer);
      if (!hitboxHit)
        return null;

      DamageController damageController = hit.transform.GetComponentInParent<DamageController>();
      if (damageController != null)
      {
        // this will cause issues if we are for example hitting targets with shotgun
        // we will receive more hits than shots fired
        Hitbox hitbox = hit.collider.transform.GetComponent<Hitbox>();
        HitArea hitArea = hitbox ? hitbox.HitArea : HitArea.Chest;
        float falloffMultiplier = raycastAttack.rangeDamageFalloff.Evaluate(hit.distance / raycastAttack.range);
        float damageAmount = raycastAttack.Damage;
        DamageData damageData = new DamageData()
        {
          Attacker = raycastAttack.Combatant,
          Element = raycastAttack.Element,
          Collider = hit.collider,
          Effects = SetupDamageEffects(raycastAttack.DamageEffects, raycastAttack.Combatant, damageAmount),
          HitAngle = Vector3.Angle((raycastAttack.Weapon.transform.position - hit.transform.position).normalized, hit.transform.forward),
          Force = raycastAttack.Combatant.aimTransform.forward * raycastAttack.pushForce,
          Position = hit.point,
          Normal = hit.normal,
          HitArea = hitArea,
          DamageAmount = hitArea.GetMultiplier() * damageAmount * falloffMultiplier,
          ImpactEffect = hitArea.GetImpactEffect(damageController)
        };

        damageController.ReceiveAttack(damageData);
        raycastAttack.Combatant.SuccessfulHit(damageData);
        return damageData;
      }

      return null;
    }

    public static DamageData ProjectileHit(ProjectileData projectileData, Vector3 position, Collision collision)
    {
      AddForce(projectileData, position, collision);

      bool hitboxHit = projectileData.Attacker.teamController.Hitboxes.IsInLayerMask(collision.collider.gameObject.layer);
      if (!hitboxHit)
        return null;

      Hitbox hitbox = collision.collider.transform.GetComponent<Hitbox>();
      HitArea hitArea = hitbox ? hitbox.HitArea : HitArea.Chest;
      DamageController damageController = collision.transform.GetComponentInParent<DamageController>();
      if (damageController != null)
      {
        DamageData damageData = new DamageData()
        {
          Attacker = projectileData.Attacker,
          Element = projectileData.Element,
          HitAngle = Vector3.Angle((position - collision.transform.position).normalized, collision.transform.forward),
          Effects = SetupDamageEffects(projectileData.DamageEffects, projectileData.Attacker, projectileData.Damage),
          Force = projectileData.Forward * projectileData.Force,
          Position = collision.GetContact(0).point,
          Normal = collision.GetContact(0).normal,
          HitArea = hitArea,
          DamageAmount = hitArea.GetMultiplier() * projectileData.Damage,
          ImpactEffect = hitArea.GetImpactEffect(damageController)
        };
        damageController.ReceiveAttack(damageData);
        projectileData.Attacker.SuccessfulHit(damageData);

        return damageData;
      }
      return null;
    }

    public static DamageData CollisionHit(Hitbox hitbox, Collision collision)
    {
      HitArea hitArea = hitbox ? hitbox.HitArea : HitArea.Chest;
      DamageController damageController = hitbox.GetComponentInParent<DamageController>();
      float damage = collision.impulse.magnitude;
      if (damageController != null)
      {
        DamageData damageData = new DamageData()
        {
          Attacker = null,
          Element = Element.None,
          HitAngle = Vector3.Angle((collision.GetContact(0).point - collision.transform.position).normalized, collision.transform.forward),
          Force = -collision.impulse,
          Position = collision.GetContact(0).point,
          Normal = collision.GetContact(0).normal,
          HitArea = hitArea,
          DamageAmount = hitArea.GetMultiplier() * damage,
          ImpactEffect = hitArea.GetImpactEffect(damageController)
        };
        damageController.ReceiveAttack(damageData);

        return damageData;
      }
      return null;
    }

    public static void Explosion(ProjectileData projectileData, Vector3 position, Collision collision = null)
    {
      if (collision != null)
      {
        position = collision.GetContact(0).point;
      }

      TeamController teamController = projectileData.Attacker.teamController;
      Collider[] colliders = Physics.OverlapSphere(position, projectileData.ExplosionRadius, teamController.Hostiles | 1 << 0);

      List<Rigidbody> rigidBodies = new List<Rigidbody>();
      HashSet<DamageController> targets = new HashSet<DamageController>();
      foreach (Collider col in colliders)
      {
        if (col.TryGetComponent(out Rigidbody rigidbody) && !rigidBodies.Contains(rigidbody))
          rigidBodies.Add(rigidbody);

        DamageController damageController = col.GetComponentInParent<DamageController>();
        if (damageController != null)
          targets.Add(damageController);
      }


      foreach (Rigidbody rigidbody in rigidBodies)
        rigidbody.AddExplosionForce(projectileData.PushForce, position, projectileData.ExplosionRadius + 1, 0.5f, ForceMode.Impulse);

      foreach (DamageController damageController in targets)
      {
        float distance = Vector3.Distance(damageController.transform.position, position);
        if (distance > projectileData.ExplosionRadius + 1)
          continue;

        float distanceMultiplier = Mathf.Clamp01(1 - Mathf.Pow(distance / (projectileData.ExplosionRadius + 1), 2));

        float damageAmount = projectileData.Damage * distanceMultiplier;
        Vector3 targetPosition = damageController.transform.position;
        DamageData damageData = new DamageData()
        {
          Attacker = projectileData.Attacker,
          Element = projectileData.Element,
          Effects = SetupDamageEffects(projectileData.DamageEffects, projectileData.Attacker, projectileData.Damage),
          HitAngle = Vector3.Angle((position - targetPosition).normalized, damageController.transform.forward),
          Force = (targetPosition - position).normalized.With(y: 0.5f) * projectileData.PushForce * distanceMultiplier,
          Position = position,
          DamageAmount = damageAmount
        };
        damageController.ReceiveAttack(damageData);
        projectileData.Attacker.SuccessfulHit(damageData);
      }
    }

    static void AddForce(ProjectileData projectileData, Vector3 position, Collision collision)
    {
      Vector3 direction = projectileData.Forward;
      Rigidbody rigidBody = collision.transform.GetComponent<Rigidbody>();
      if (rigidBody == null)
        return;

      if (direction == Vector3.zero)
        direction = -(position - collision.collider.transform.position).normalized;
      rigidBody.AddForce(direction * projectileData.PushForce, ForceMode.Impulse);
    }

    static void AddForce(RaycastAttack raycastAttack, RaycastHit hit)
    {
      Rigidbody rigidBody = hit.transform.GetComponent<Rigidbody>();
      if (rigidBody != null)
      {
        rigidBody.AddForce(raycastAttack.Combatant.aimTransform.forward * raycastAttack.pushForce, ForceMode.Impulse);
      }
    }
  }
}
