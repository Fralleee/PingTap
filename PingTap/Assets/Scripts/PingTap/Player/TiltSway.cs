using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.PingTap
{
	public class TiltSway : MonoBehaviour
	{
		[SerializeField] float smoothRotation = 10f;

		[Header("Strafe rotation")]
		[SerializeField] float strafeRotationAmount = 1.6f;
		[SerializeField] float maxStrafeRotation = 5f;

		[Header("Look rotation")]
		[SerializeField] float lookRotationAmount = 0.6f;
		[SerializeField] float maxLookRotation = 3f;

		Quaternion initialRotation;
		float LookX;
		float StrafeX;

		void Start()
		{
			initialRotation = transform.localRotation;
		}

		void Update()
		{
			CalculateSway();

			float strafeAmount = Mathf.Clamp(StrafeX * strafeRotationAmount, -maxStrafeRotation, maxStrafeRotation);
			float lookAmount = Mathf.Clamp(LookX * lookRotationAmount, -maxLookRotation, maxLookRotation);

			Quaternion zRot = RotateOnStrafe(strafeAmount);
			Quaternion yRot = RotateOnLook(lookAmount);

			Quaternion finalRotation = zRot * yRot * initialRotation;
			transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRotation, Time.deltaTime * smoothRotation);
		}

		void CalculateSway()
		{
			LookX = -Input.GetAxis("Mouse X");
			StrafeX = -Input.GetAxis("Horizontal");
		}

		Quaternion RotateOnStrafe(float amount) => Quaternion.Euler(new Vector3(0f, 0f, amount));
		Quaternion RotateOnLook(float amount) =>  Quaternion.Euler(new Vector3(0f, -amount, 0f));
	}
}
