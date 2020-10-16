using UnityEngine;

namespace Fralle.Gameplay
{
	public class CameraManager : MonoBehaviour
	{
		[SerializeField] Camera sceneCamera;

		public static void ConfigureCursor(bool doLock = true)
		{
			if (doLock)
			{
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
			}
			else
			{

				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
		}

		public void ActivateSceneCamera()
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;

			if (sceneCamera)
			{
				Player.Disable();
				sceneCamera.gameObject.SetActive(true);
				sceneCamera.GetComponent<AudioListener>().enabled = true;
			}
		}
	}

}
