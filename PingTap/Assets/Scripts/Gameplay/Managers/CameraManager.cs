using UnityEngine;

namespace Fralle.Gameplay
{
  public class CameraManager : MonoBehaviour
  {
    [SerializeField] Camera sceneCamera;

    public void ActivateSceneCamera()
    {
      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.None;

      if (!sceneCamera)
        return;

      Player.Disable();
      sceneCamera.gameObject.SetActive(true);
      sceneCamera.GetComponent<AudioListener>().enabled = true;
    }
  }

}
