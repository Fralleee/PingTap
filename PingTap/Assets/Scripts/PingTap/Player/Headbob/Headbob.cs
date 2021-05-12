using CombatSystem;
using CombatSystem.Combat;
using CombatSystem.Enums;
using Fralle.FpsController;
using UnityEngine;

namespace Fralle.PingTap
{
	public class Headbob : MonoBehaviour
	{
		[SerializeField] HeadbobConfiguration defaultConfiguration;
		public HeadbobConfiguration overrideConfguration;

		[SerializeField] Transform cameraTransform;
		[SerializeField] Transform weaponTransform;

		PlayerController playerController;
		Combatant combatant;
		HeadbobConfiguration configuration => overrideConfguration ?? defaultConfiguration;
		Vector3 localAxis = new Vector3(0, 1, 0);
		Vector3 initPos;
		Quaternion initRot;
		float timer;
		float angleChanges;
		bool pause;

		void Awake()
		{
			playerController = GetComponentInParent<PlayerController>();
			combatant = GetComponentInParent<Combatant>();

			combatant.OnWeaponSwitch += OnWeaponSwitch;
			if (combatant.EquippedWeapon != null)
				OnWeaponSwitch(combatant.EquippedWeapon, null);
		}

		void OnWeaponSwitch(Weapon newWeapon, Weapon oldWeapon)
		{
			if (oldWeapon != null)
				oldWeapon.OnActiveWeaponActionChanged -= OnWeaponActionChanged;

			if (newWeapon != null)
				newWeapon.OnActiveWeaponActionChanged += OnWeaponActionChanged;
		}

		void OnWeaponActionChanged(Status status)
		{
			pause = status == Status.Firing;
		}

		void Start()
		{
			initPos = cameraTransform.localPosition;
			initRot = weaponTransform.localRotation;
		}

		void Update()
		{
			if (pause)
			{
				cameraTransform.localPosition = initPos;
				angleChanges = 0;
				timer = 0;
				return;
			}

			float bob = 0f;
			float curvePosition = 0f;
			Vector3 calcPosition = cameraTransform.localPosition;

			if (!playerController.IsMoving)
				timer = 0;
			else
			{
				curvePosition = Mathf.Sin(timer);
				bob = -Mathf.Abs(Mathf.Abs(curvePosition) - 1);

				timer += configuration.BobbingSpeed * Time.deltaTime;

				if (timer > Mathf.PI * 2)
					timer -= (Mathf.PI * 2);
			}

			if (bob != 0)
			{
				bob *= playerController.ModifiedMovementSpeed;
				calcPosition.y = initPos.y + configuration.CameraBobbingAmount + bob * configuration.CameraBobbingAmount;
			}
			else
				calcPosition.y = initPos.y;

			if (curvePosition != 0)
				angleChanges = initRot.eulerAngles.y + curvePosition * playerController.ModifiedMovementSpeed * configuration.WeaponRotationAmount;
			else
				angleChanges = Mathf.LerpAngle(angleChanges, 0, Time.deltaTime * 6f);

			cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, calcPosition, Time.deltaTime * 6f);
		}

		void LateUpdate()
		{
			Quaternion change = Quaternion.AngleAxis(angleChanges, localAxis);
			weaponTransform.localRotation = initRot * change;
		}
	}

}
