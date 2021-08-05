using Fralle.Core;
using Fralle.Core.CameraControls;
using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Fralle.PingTap
{
	[Serializable]
	public class DamageIndicatorHandler
	{
		[SerializeField] VolumeProfile postProcess;
		[SerializeField] ShakeTransformEventData bodyshotShake;
		[SerializeField] ShakeTransformEventData headshotShake;

		DamageController damageController;
		ShakeTransform cameraShakeTransform;
		Volume damageVolume;

		bool invalidConfiguration => postProcess == null || bodyshotShake == null || headshotShake == null;

		public void Setup(DamageController damageController, PostProcessController postProcessController)
		{
			if (invalidConfiguration)
				return;

			this.damageController = damageController;
			this.damageController.OnReceiveAttack += HandleReceiveAttack;
			this.damageController.OnHealthChange += HandleHealthChange;

			damageVolume = postProcessController.AddProfile(postProcess);

			cameraShakeTransform = damageController.GetComponentInChildren<ShakeTransform>();
		}

		public void Clean()
		{
			if (invalidConfiguration)
				return;

			damageController.OnReceiveAttack -= HandleReceiveAttack;
			damageController.OnHealthChange -= HandleHealthChange;
		}

		void HandleHealthChange(float currentHealth, float totalHealth)
		{
			damageVolume.weight = 1 - currentHealth / totalHealth;
		}

		void HandleReceiveAttack(DamageController dc, DamageData dd)
		{
			if (dd.HitArea == HitArea.Head)
				cameraShakeTransform.AddShakeEvent(headshotShake);
			else
				cameraShakeTransform.AddShakeEvent(bodyshotShake);
		}
	}
}
