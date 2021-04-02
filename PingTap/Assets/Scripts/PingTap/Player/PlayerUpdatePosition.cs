using Fralle.FpsController;
using UnityEngine;

namespace Fralle
{
	public class PlayerUpdatePosition : MonoBehaviour
	{
		[SerializeField] float rotateOnAngle = 35;

		[SerializeField] Vector3 positionOffset;
		[SerializeField] Transform body;
		[SerializeField] Transform orientation;

		PlayerController playerController;
		bool doRotate;

		void Awake()
		{
			playerController = GetComponentInParent<PlayerController>();
		}

		void Update()
		{
			transform.position = body.position + positionOffset;

			UpdateRotation();
		}

		void UpdateRotation()
		{



			if (playerController.IsMoving)
			{
				transform.rotation = orientation.rotation;
				doRotate = false;
			}
			else if (Vector3.Angle(orientation.forward, transform.forward) > rotateOnAngle)
			{
				doRotate = true;

				// do rotate
				//Quaternion.RotateTowards(transform.rotation, orientation.rotation, Time.deltaTime * 10f);

				//transform.rotation = orientation.rotation;
			}

			if (doRotate)
			{
				Debug.Log("Rotate?");
				transform.rotation = Quaternion.Lerp(transform.rotation, orientation.rotation, Time.deltaTime * 10f);
				if (Vector3.Angle(orientation.forward, transform.forward) < 1f)
					doRotate = false;
			}
		}
	}
}
