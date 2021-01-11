using UnityEngine;

namespace Fralle.Core.CameraControls
{
	public class BillboardFaceCamera : MonoBehaviour
	{
		Camera mainCamera;

		void Start()
		{
			mainCamera = Camera.main;
		}

		void LateUpdate()
		{
			if (!mainCamera)
				return;

			FaceCamera();
		}

		void FaceCamera()
		{
			transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
		}
	}
}
