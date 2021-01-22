using CombatSystem.Addons;
using CombatSystem.Combat;
using CombatSystem.Enums;
using CombatSystem.Interfaces;
using System;
using UnityEngine;

namespace CombatSystem
{
	public class Weapon : MonoBehaviour, IWeapon
	{
		public event Action<Status> OnActiveWeaponActionChanged = delegate { };

		[Header("Weapon")]
		public string weaponName;
		[SerializeField] float equipAnimationTime = 0.3f;
		public Transform[] muzzles;

		public Status ActiveWeaponAction { get; private set; }

		[HideInInspector] public Combatant combatant;
		[HideInInspector] public RecoilAddon recoilAddon;
		[HideInInspector] public AmmoAddon ammoAddonController;

		public bool isEquipped { get; private set; }

		public float nextAvailableShot;

		float equipTime;
		bool animationComplete;
		Vector3 startPosition;
		Quaternion startRotation;

		void Update()
		{
			AnimateEquip();

			if (ActiveWeaponAction == Status.Firing)
			{
				nextAvailableShot -= Time.deltaTime;
				if (nextAvailableShot <= 0)
					ChangeWeaponAction(Status.Ready);
			}
			else
			{
				nextAvailableShot = 0;
			}
		}

		public void Equip(Combatant combatant, bool shouldAnimate = true)
		{
			if (string.IsNullOrWhiteSpace(weaponName))
				weaponName = name;

			ActiveWeaponAction = Status.Equipping;
			equipTime = 0f;
			animationComplete = !shouldAnimate;
			this.combatant = combatant;

			startPosition = shouldAnimate ? transform.localPosition : Vector3.zero;
			startRotation = shouldAnimate ? transform.localRotation : Quaternion.identity;

			recoilAddon = GetComponent<RecoilAddon>();
			ammoAddonController = GetComponent<AmmoAddon>();

			isEquipped = true;
		}

		public void ChangeWeaponAction(Status newActiveWeaponAction)
		{
			ActiveWeaponAction = newActiveWeaponAction;
			OnActiveWeaponActionChanged(newActiveWeaponAction);
		}

		void AnimateEquip()
		{
			if (animationComplete)
				return;

			var isEquipping = equipTime < equipAnimationTime;
			if (!isEquipping)
			{
				animationComplete = true;
				ActiveWeaponAction = Status.Ready;
				return;
			}

			equipTime += Time.deltaTime;
			equipTime = Mathf.Clamp(equipTime, 0f, equipAnimationTime);
			var delta = -(Mathf.Cos(Mathf.PI * (equipTime / equipAnimationTime)) - 1f) / 2f;
			transform.localPosition = Vector3.Lerp(startPosition, Vector3.zero, delta);
			transform.localRotation = Quaternion.Lerp(startRotation, Quaternion.identity, delta);
		}
	}
}
