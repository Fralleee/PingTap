using UnityEngine;

public class DebugDrawLine : MonoBehaviour
{
	[SerializeField] Color color = Color.red;
	[SerializeField] float distance = 50f;

	void OnDrawGizmos()
	{
		Gizmos.color = color;
		Gizmos.DrawLine(transform.position, transform.forward * distance);
	}
}
