using UnityEngine;

namespace Fralle.Core.CameraControls
{
	public class CameraFollowTransform : MonoBehaviour
	{

		public Transform transformToFollow;

		void LateUpdate()
		{
			transform.position = transformToFollow.position;
		}
	}
}
