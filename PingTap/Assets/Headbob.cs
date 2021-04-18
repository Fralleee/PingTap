using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headbob : MonoBehaviour
{
	[Range(0.01f, 1f)] public float bobbingSpeed = 0.08f;
	[Range(0.01f, 1f)] public float bobbingAmount = 0.05f;
	[Range(0.1f, 3f)] public float bobbingRotationAmount = 1.75f;
	[Range(0.1f, 3f)] public float vertBobbingAmount = 1.25f;

	Vector3 initPos;
	Quaternion initRot;
	float timer;
	float yNormalizer;

	public Vector3 localAxis = new Vector3(0, 1, 0);
	public float angleChanges = 0f;

	void Start()
	{
		initPos = transform.localPosition;
		initRot = transform.localRotation;
		yNormalizer = (bobbingAmount * vertBobbingAmount) / 4f;
	}

	void Update()
	{
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		float aHorz = Mathf.Abs(horizontal);
		float aVert = Mathf.Abs(vertical);

		float xMovement = 0.0f;
		float yMovement = 0.0f;
		float yRotation = 0.0f;

		Vector3 calcPosition = transform.localPosition;
		float rot = transform.localRotation.eulerAngles.y;

		// If no movement
		if (aHorz == 0 && aVert == 0)
		{
			timer = 0;
		}
		else
		{
			xMovement = Mathf.Sin(timer);
			yMovement = -Mathf.Abs(Mathf.Abs(xMovement) - 1);
			yRotation = Mathf.Sin(timer);

			timer += bobbingSpeed;

			if (timer > Mathf.PI * 2)
			{
				timer = timer - (Mathf.PI * 2);
			}
		}

		float totalMovement = Mathf.Clamp(aVert + aHorz, 0, 1);
		if (yMovement != 0)
		{
			yMovement = yMovement * totalMovement;
			calcPosition.y = initPos.y + yNormalizer + yMovement * bobbingAmount * vertBobbingAmount;
		}
		else
		{
			calcPosition.y = initPos.y;
		}

		if (yRotation != 0)
		{
			rot = initRot.y + yRotation * totalMovement * bobbingRotationAmount;
		}
		else
		{
			rot = initRot.y;
		}

		angleChanges = rot;
		transform.localPosition = Vector3.Lerp(transform.localPosition, calcPosition, Time.deltaTime * 6f);
	}
	
	void LateUpdate()
	{
		Quaternion change = Quaternion.AngleAxis(angleChanges, localAxis);
		transform.localRotation = initRot * change;
	}
	void OnValidate()
	{
		yNormalizer = (bobbingAmount * vertBobbingAmount) / 4f;
	}
}
