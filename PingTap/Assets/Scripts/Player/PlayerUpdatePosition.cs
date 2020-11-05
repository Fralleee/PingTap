using UnityEngine;

namespace Fralle
{
	public class PlayerUpdatePosition : MonoBehaviour
	{
		[SerializeField] Vector3 positionOffset;
		[SerializeField] Transform body;
		[SerializeField] Transform orientation;

		void Update()
		{
			transform.position = body.position + positionOffset;
			transform.rotation = orientation.rotation;
		}
	}
}
