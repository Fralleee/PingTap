using CombatSystem;
using CombatSystem.Combat;
using CombatSystem.Enums;
using Fralle.FpsController;
using UnityEngine;

namespace Fralle.PingTap
{
	public class Headbob : MonoBehaviour
	{
		[Header("Configurations")]
		[SerializeField] HeadbobConfiguration defaultConfiguration;
		public HeadbobConfiguration overrideConfguration;

		[Header("Transforms")]
		[SerializeField] Transform cameraTransform;
		[SerializeField] Transform weaponTransform;

		[Header("Speed")]
		[SerializeField] float smoothSpeed = 10f;

		[Header("Strafe rotation")]
		[SerializeField] float strafeRotationAmount = 1.6f;
		[SerializeField] float maxStrafeRotation = 5f;

		PlayerController playerController;
		Combatant combatant;
		HeadbobConfiguration configuration => overrideConfguration ?? defaultConfiguration;

		Vector3 localAxis = new Vector3(0, 1, 0);
		Vector3 cameraInitPos;
		Vector3 weaponInitPos;
		Quaternion initRotation;

		float timer;
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
			weaponInitPos = cameraTransform.localPosition;
			initRotation = weaponTransform.localRotation;
		}

		void Update()
		{
			if (pause || !playerController.IsMoving)
			{
				Reset();
				return;
			}

			ApplyModifiers(out Vector3 cameraCalcPosition, out Vector3 weaponCalcPosition, out Quaternion bobRot, out Quaternion strafeRot);
			ApplyMotion(cameraCalcPosition, weaponCalcPosition, bobRot, strafeRot);

			UpdateTimer();
		}

		void ApplyModifiers(out Vector3 cameraCalcPosition, out Vector3 weaponCalcPosition, out Quaternion bobRot, out Quaternion strafeRot)
		{
			float curvePosition = Mathf.Sin(timer);
			float bob = Mathf.Abs(curvePosition);
			float strafeX = -playerController.Movement.x;

			cameraCalcPosition = Vector3.zero;
			weaponCalcPosition = Vector3.zero;

			bob *= playerController.ModifiedMovementSpeed * 0.1f;
			cameraCalcPosition.y = bob * configuration.CameraBobbingAmount;
			weaponCalcPosition.y = bob * configuration.WeaponBobbingAmount;

			float angleChanges = initRotation.eulerAngles.y + curvePosition * playerController.ModifiedMovementSpeed * 0.1f * configuration.WeaponRotationAmount;
			float strafeAmount = Mathf.Clamp(strafeX * strafeRotationAmount, -maxStrafeRotation, maxStrafeRotation);

			bobRot = Quaternion.AngleAxis(angleChanges, localAxis);
			strafeRot = Quaternion.Euler(new Vector3(0f, 0f, strafeAmount));
		}

		void ApplyMotion(Vector3 cameraCalcPosition, Vector3 weaponCalcPosition, Quaternion bobRot, Quaternion strafeRot)
		{
			cameraTransform.localPosition = cameraCalcPosition;
			weaponTransform.localPosition = weaponCalcPosition;
			weaponTransform.localRotation = initRotation * bobRot * strafeRot;
		}

		void Reset()
		{
			timer = 0;
			weaponTransform.localRotation = Quaternion.Lerp(weaponTransform.localRotation, initRotation, Time.deltaTime * smoothSpeed);
			cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, cameraInitPos, Time.deltaTime * smoothSpeed);
			weaponTransform.localPosition = Vector3.Lerp(weaponTransform.localPosition, weaponInitPos, Time.deltaTime * smoothSpeed);
		}

		void UpdateTimer()
		{
			timer += configuration.BobbingSpeed * Time.deltaTime;
			if (timer > Mathf.PI * 2)
				timer -= (Mathf.PI * 2);
		}
	}

}
