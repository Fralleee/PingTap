using CombatSystem.Defense;
using CombatSystem.Effect;
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

		[HideInInspector] public List<DamageEffect> damageEffects = new List<DamageEffect>();
		[HideInInspector] public bool isDead;

		[Header("UI")]
		[SerializeField] GameObject healthbarPrefab;
		[SerializeField] GameObject floatingNumbersPrefab;

		[Header("Stats")]
		public float currentHealth;
		public float maxHealth = 100f;
		public bool immortal;
		public Armor armor;

		void Awake()
		{
			SetupUI();
		}

		void Start()
		{
			if (currentHealth.EqualsWithTolerance(0f))
				currentHealth = maxHealth;
		}

		void Update()
		{
			if (isDead)
				return;
			for (var i = 0; i < damageEffects.Count; i++)
			{
				damageEffects[i].Tick(this);
				if (damageEffects[i].timer <= damageEffects[i].time)
					continue;
				damageEffects[i].Exit(this);
				damageEffects.RemoveAt(i);
			}
		}

		public void ReceiveAttack(DamageData damageData)
		{
			if (isDead)
				return;

			OnReceiveAttack(this, damageData);
			damageData = armor.Protect(damageData, this);
			TakeDamage(damageData);
			ApplyEffects(damageData);
		}

		public void TakeDamage(DamageData damageData)
		{
			if (isDead)
				return;

			damageData.victim = this;

			if (damageData.damageAmount <= 0)
			{
				damageData.attacker?.Stats.OnSuccessfulAttack(damageData);
				return;
			}

			currentHealth -= damageData.damageAmount;
			OnHealthChange(currentHealth, maxHealth);
			OnDamageTaken(this, damageData);
			if (currentHealth <= 0)
			{
				damageData.killingBlow = true;
				damageData.gib = currentHealth <= -maxHealth * 0.5f;
				Death(damageData);
			}

			damageData.attacker?.Stats.OnSuccessfulAttack(damageData);
		}

		void ApplyEffects(DamageData damageData)
		{
			foreach (var t in damageData.effects)
			{
				var effect = t;
				var oldEffect = damageEffects.FirstOrDefault(x => x.name == effect.name);
				effect = effect.Append(oldEffect);
				effect.Enter(this);
				damageEffects.Upsert(oldEffect, effect);
			}
		}

		void Death(DamageData damageData)
		{
			if (isDead)
				return;

			if (immortal)
			{
				currentHealth = maxHealth;
				OnHealthChange(currentHealth, maxHealth);
			}
			else
			{
				isDead = true;
				OnDeath(this, damageData);
			}
		}

		void SetupUI()
		{
			var uiTransform = transform.Find("UI");
			if (healthbarPrefab)
				Instantiate(healthbarPrefab, uiTransform);
			if (floatingNumbersPrefab)
				Instantiate(floatingNumbersPrefab, uiTransform);
		}
	}
}
