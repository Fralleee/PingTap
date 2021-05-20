using Fralle.Core.Extensions;
using Fralle.FpsController;
using UnityEngine;

public class Headtilt : MonoBehaviour
{
	[Header("Speed")]
	[SerializeField] float smoothSpeed = 10f;

	[Header("Strafe rotation")]
	[SerializeField] float strafeRotationAmount = 1.6f;
	[SerializeField] float maxStrafeRotation = 5f;

	PlayerController playerController;

	Quaternion initRotation;

	void Awake()
	{
		playerController = GetComponentInParent<PlayerController>();		
	}

	void Start()
	{
		initRotation = transform.localRotation;
	}

	void Update()
	{
		if (playerController.IsMoving && !playerController.Movement.x.EqualsWithTolerance(0f))
		{
			float strafeAmount = Mathf.Clamp(-playerController.Movement.x * strafeRotationAmount, -maxStrafeRotation, maxStrafeRotation);
			Quaternion strafeRot = Quaternion.Euler(new Vector3(0f, 0f, strafeAmount));
			transform.localRotation = Quaternion.Lerp(transform.localRotation, initRotation * strafeRot, Time.deltaTime * smoothSpeed);
		}
		else {
			transform.localRotation = Quaternion.Lerp(transform.localRotation, initRotation, Time.deltaTime * smoothSpeed);
		}
	}


}
