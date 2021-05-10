using CombatSystem;
using CombatSystem.Combat;
using CombatSystem.Enums;
using Fralle.FpsController;
using UnityEngine;

namespace Fralle
{
	public class Headbob : MonoBehaviour
	{
		[Range(0.01f, 1f)] public float BobbingSpeed = 0.08f;
		[Range(0.01f, 1f)] public float BobbingAmount = 0.05f;
		[Range(0f, 3f)] public float BobbingRotationAmount = 1.75f;
		[Range(0.1f, 3f)] public float VertBobbingAmount = 1.25f;

		PlayerController playerController;
		Combatant combatant;
		Vector3 localAxis = new Vector3(0, 1, 0);
		Vector3 initPos;
		Quaternion initRot;
		float timer;
		float yNormalizer;
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
			pause = status != Status.Ready;
		}

		void Start()
		{
			initPos = transform.localPosition;
			initRot = transform.localRotation;
			yNormalizer = (BobbingAmount * VertBobbingAmount);
		}

		void Update()
		{
			if (pause)
			{
				angleChanges = 0;
				transform.localPosition = initPos;
				timer = 0;
				return;
			}

			float bob = 0f;
			float curvePosition = 0f;
			Vector3 calcPosition = transform.localPosition;

			if (!playerController.IsMoving)
				timer = 0;
			else
			{
				curvePosition = Mathf.Sin(timer);
				bob = -Mathf.Abs(Mathf.Abs(curvePosition) - 1);

				timer += BobbingSpeed;

				if (timer > Mathf.PI * 2)
					timer = timer - (Mathf.PI * 2);
			}

			if (bob != 0)
			{
				bob *= playerController.ModifiedMovementSpeed;
				calcPosition.y = initPos.y + yNormalizer + bob * BobbingAmount * VertBobbingAmount;
			}
			else
				calcPosition.y = initPos.y;

			if (curvePosition != 0)
				angleChanges = initRot.eulerAngles.y + curvePosition * playerController.ModifiedMovementSpeed * BobbingRotationAmount;
			else
				angleChanges = Mathf.LerpAngle(angleChanges, 0, Time.deltaTime * 6f);

			transform.localPosition = Vector3.Lerp(transform.localPosition, calcPosition, Time.deltaTime * 6f);
		}

		void LateUpdate()
		{
			Quaternion change = Quaternion.AngleAxis(angleChanges, localAxis);
			transform.localRotation = initRot * change;
		}

		void OnValidate()
		{
			yNormalizer = (BobbingAmount * VertBobbingAmount);
		}
	}

}
