using CombatSystem.Action;
using CombatSystem.Defense;
using CombatSystem.Effect;
using CombatSystem.Enums;
using CombatSystem.Interfaces;
using Fralle.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CombatSystem.Combat.Damage
{
	public class DamageController : MonoBehaviour, IDamageable
	{
		public event Action<DamageController, DamageData> OnDeath = delegate { };
		public event Action<DamageController, DamageData> OnDamageTaken = delegate { };
		public event Action<DamageController, DamageData> OnReceiveAttack = delegate { };
		public event Action<float, float> OnHealthChange = delegate { };

		[HideInInspector] public List<DamageEffect> DamageEffects = new List<DamageEffect>();
		[HideInInspector] public bool IsDead;

		[Header("UI")]
		[SerializeField] GameObject healthbarPrefab;
		[SerializeField] GameObject floatingNumbersPrefab;

		[Header("Stats")]
		public float CurrentHealth;
		public float MaxHealth = 100f;
		public bool Immortal;
		public Armor Armor;

		[Header("Effects")]
		public GameObject MajorImpactEffect;
		public GameObject NerveImpactEffect;

		[Header("Audio")]
		[SerializeField] AudioClip damageSound;
		[SerializeField] AudioClip deathSound;

		AudioSource audioSource;

		void Awake()
		{
			audioSource = GetComponent<AudioSource>();

			SetupUi();
		}

		void Start()
		{
			if (CurrentHealth.EqualsWithTolerance(0f))
				CurrentHealth = MaxHealth;
		}

		void Update()
		{
			if (IsDead)
				return;
			for (var i = 0; i < DamageEffects.Count; i++)
			{
				DamageEffects[i].Tick(this);
				if (DamageEffects[i].Timer <= DamageEffects[i].Time)
					continue;
				DamageEffects[i].Exit(this);
				DamageEffects.RemoveAt(i);
			}
		}

		public void ReceiveAttack(DamageData damageData)
		{
			if (IsDead)
				return;

			OnReceiveAttack(this, damageData);
			damageData = Armor.Protect(damageData, this);
			TakeDamage(damageData);
			ApplyEffects(damageData);
		}

		public void ReceiveAttack(RaycastAttack raycastAttack, RaycastHit hit)
		{
			if (IsDead)
				return;

			var hitbox = hit.transform.GetComponent<Hitbox>();
			var hitArea = hitbox ? hitbox.HitArea : HitArea.Major;
			var damageData = new DamageData()
			{
				Attacker = raycastAttack.Combatant,
				Element = raycastAttack.Element,
				Effects = DamageEffects.Select(x => x.Setup(raycastAttack.Combatant, raycastAttack.Damage)).ToArray(),
				HitAngle = Vector3.Angle((raycastAttack.Weapon.transform.position - hit.transform.position).normalized, hit.transform.forward),
				Force = raycastAttack.Combatant.AimTransform.forward * raycastAttack.PushForce,
				Position = hit.point,
				HitArea = hitArea,
				DamageAmount = hitArea.GetMultiplier() * raycastAttack.Damage
			};

			OnReceiveAttack(this, damageData);
			damageData = Armor.Protect(damageData, this);
			TakeDamage(damageData);
			ApplyEffects(damageData);
		}

		public void TakeDamage(DamageData damageData)
		{
			if (IsDead)
				return;

			damageData.Victim = this;

			if (damageData.DamageAmount <= 0)
			{
				damageData.Attacker?.Stats.OnSuccessfulAttack(damageData);
				return;
			}

			CurrentHealth -= damageData.DamageAmount;
			OnHealthChange(CurrentHealth, MaxHealth);
			OnDamageTaken(this, damageData);
			if (CurrentHealth <= 0)
			{
				damageData.KillingBlow = true;
				damageData.Gib = CurrentHealth <= -MaxHealth * 0.5f;
				Death(damageData);
			}
			else if (audioSource && damageSound)
			{
				audioSource.clip = damageSound;
				audioSource.Play();
			}

			damageData.Attacker?.Stats.OnSuccessfulAttack(damageData);
		}

		void ApplyEffects(DamageData damageData)
		{
			foreach (var t in damageData.Effects)
			{
				var effect = t;
				var oldEffect = DamageEffects.FirstOrDefault(x => x.name == effect.name);
				effect = effect.Append(oldEffect);
				effect.Enter(this);
				DamageEffects.Upsert(oldEffect, effect);
			}
		}

		void Death(DamageData damageData)
		{
			if (IsDead)
				return;

			if (audioSource && deathSound)
			{
				audioSource.clip = deathSound;
				audioSource.Play();
			}
			if (Immortal)
			{
				CurrentHealth = MaxHealth;
				OnHealthChange(CurrentHealth, MaxHealth);
			}
			else
			{
				IsDead = true;
				OnDeath(this, damageData);
			}
		}

		void SetupUi()
		{
			var uiTransform = transform.Find("UI");
			if (healthbarPrefab)
				Instantiate(healthbarPrefab, uiTransform);
			if (floatingNumbersPrefab)
				Instantiate(floatingNumbersPrefab, uiTransform);
		}
	}
}
