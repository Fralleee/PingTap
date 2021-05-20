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

		[SerializeField] float smoothSpeed = 10f;

		PlayerController playerController;
		Combatant combatant;
		HeadbobConfiguration configuration => overrideConfguration ?? defaultConfiguration;
		Vector3 localAxis = new Vector3(0, 1, 0);
		Vector3 cameraInitPos;
		Vector3 weaponInitPos;
		Quaternion initRot;
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
			initRot = weaponTransform.localRotation;
		}

		void Update()
		{
			if (pause || !playerController.IsMoving)
			{
				Reset();
				return;
			}

			float curvePosition = Mathf.Sin(timer);
			float bob = Mathf.Abs(curvePosition);

			ApplyModifiers(curvePosition, bob, out Vector3 cameraCalcPosition, out Vector3 weaponCalcPosition, out float angleChanges);
			ApplyMotion(cameraCalcPosition, weaponCalcPosition, angleChanges);

			UpdateTimer();
		}

		void ApplyModifiers(float curvePosition, float bob, out Vector3 cameraCalcPosition, out Vector3 weaponCalcPosition, out float angleChanges)
		{
			cameraCalcPosition = Vector3.zero;
			weaponCalcPosition = Vector3.zero;

			bob *= playerController.ModifiedMovementSpeed * 0.1f;
			cameraCalcPosition.y = bob * configuration.CameraBobbingAmount;
			weaponCalcPosition.y = bob * configuration.WeaponBobbingAmount;

			angleChanges = initRot.eulerAngles.y + curvePosition * playerController.ModifiedMovementSpeed * 0.1f * configuration.WeaponRotationAmount;
		}

		void ApplyMotion(Vector3 cameraCalcPosition, Vector3 weaponCalcPosition, float angleChanges)
		{
			cameraTransform.localPosition = cameraCalcPosition;
			weaponTransform.localPosition = weaponCalcPosition;
			weaponTransform.localRotation = initRot * Quaternion.AngleAxis(angleChanges, localAxis);
		}

		void Reset()
		{
			timer = 0;
			weaponTransform.localRotation = Quaternion.Lerp(weaponTransform.localRotation, initRot, Time.deltaTime * smoothSpeed);
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
