using Fralle.FpsController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.PingTap
{
	public class Sway : MonoBehaviour
	{
		[SerializeField] float smoothRotation = 10f;

		[Header("Look rotation")]
		[SerializeField] float lookRotationAmount = 0.6f;
		[SerializeField] float maxLookRotation = 3f;

		PlayerController playerController;

		Quaternion initialRotation;

		void Awake()
		{
			playerController = GetComponentInParent<PlayerController>();
		}

		void Start()
		{
			initialRotation = transform.localRotation;
		}

		void Update()
		{
			float lookAmountX = Mathf.Clamp(playerController.MouseLook.x * lookRotationAmount, -maxLookRotation, maxLookRotation);
			float lookAmountY = Mathf.Clamp(playerController.MouseLook.y * lookRotationAmount, -maxLookRotation, maxLookRotation);

			Quaternion finalRotation = initialRotation * Quaternion.Euler(new Vector3(lookAmountY, lookAmountX, 0f));
			transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRotation, Time.deltaTime * smoothRotation);
		}

	}
}
