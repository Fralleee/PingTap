using UnityEngine;

namespace Fralle.Targeting
{
	public class TargetDetector : MonoBehaviour
	{
		[SerializeField] LayerMask layerMask;

		readonly RaycastHit[] results = new RaycastHit[5];

		void FixedUpdate()
		{
			int hits = Physics.RaycastNonAlloc(transform.position, transform.forward, results, 100f, layerMask);
			for (int i = 0; i < hits; i++)
			{
				TargetController targetController = results[i].transform.gameObject.GetComponent<TargetController>();
				if (targetController)
				{
					targetController.RaycastHit();
				}
			}
		}

	}
}
