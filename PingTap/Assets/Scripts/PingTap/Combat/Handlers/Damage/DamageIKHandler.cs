using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Fralle.Pingtap
{
	[Serializable]
	public class DamageIKHandler
	{
		public bool enabled;
		public Transform body;
		public Transform target;
		public float resetTime = 0.25f;
		[Range(0, 1)] public float maxWeight = 0.15f;

		DamageController damageController;
		ChainIKConstraint chainIKConstraint;
		float resetTimer;

		public void Setup(DamageController damageController)
		{
			this.damageController = damageController;
			this.damageController.OnReceiveAttack += HandleReceiveAttack;
			chainIKConstraint = target.GetComponentInParent<ChainIKConstraint>();
		}

		public void Clean()
		{
			damageController.OnReceiveAttack -= HandleReceiveAttack;
		}

		public void LerpWeights()
		{
			if (resetTimer <= 0)
				return;

			float percentage = 1 - resetTimer * (1 / resetTime);
			chainIKConstraint.weight = Mathf.Lerp(maxWeight, 0, percentage);
			resetTimer -= Time.deltaTime;
		}

		void HandleReceiveAttack(DamageController dc, DamageData dd)
		{
			if (resetTimer > 0)
				return;

			Vector3 direction = dd.Force.normalized * 2f;
			target.position = body.position + direction; // .With(y: -1.5f);
			chainIKConstraint.weight = maxWeight;
			resetTimer = resetTime;
		}
	}
}
