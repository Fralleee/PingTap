using CombatSystem.Combat;
using Fralle.FpsController;
using UnityEngine;

namespace Fralle
{
	public class Sway : MonoBehaviour
	{
		[SerializeField] float swaySize = 0.004f;
		[SerializeField] float swaySmooth = 25f;
		[SerializeField] float idleSmooth = 1f;

		Combatant combatant;
		PlayerController playerController;

		Vector3 initialSwayPosition;
		Vector3 nextIdlePosition = Vector3.zero;

		bool shouldSway => combatant.equippedWeapon && !Cursor.visible;

		void Awake()
		{
			initialSwayPosition = transform.localPosition;
			nextIdlePosition = initialSwayPosition;

			combatant = GetComponentInParent<Combatant>();
			playerController = GetComponentInParent<PlayerController>();
		}

		void LateUpdate()
		{
			if (!shouldSway)
				return;

			var delta = -playerController.mouseLook;
			if (delta.magnitude > 0)
				PerformSway(delta);
			else
				PerformIdle();
		}

		void PerformSway(Vector2 delta)
		{
			transform.localPosition += (Vector3)delta * swaySize * 0.001f;
			transform.localPosition = Vector3.Lerp(transform.localPosition, initialSwayPosition, swaySmooth * Time.deltaTime);
		}

		void PerformIdle()
		{
			transform.localPosition = Vector3.Lerp(transform.localPosition, nextIdlePosition, idleSmooth * Time.deltaTime);
			if (Vector3.Distance(transform.localPosition, nextIdlePosition) < 0.005f)
				NewIdlePosition();
		}

		void NewIdlePosition()
		{
			nextIdlePosition = initialSwayPosition + (Vector3)(Random.insideUnitCircle * 0.01f);
		}

	}
}
