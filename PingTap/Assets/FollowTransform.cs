using UnityEngine;

public class FollowTransform : MonoBehaviour
{
	[SerializeField] Transform transformToFollow;
	[SerializeField] bool position;
	[SerializeField] bool rotation;

	void LateUpdate()
	{
		if (position)
			transform.position = transformToFollow.position;
		if (rotation)
			transform.rotation = transformToFollow.rotation;
	}
}
