using Fralle.FpsController;
using UnityEngine;

namespace Fralle
{
	public class PlayerAnimation : MonoBehaviour
	{
		Animator animator;
		PlayerController playerController;

		void Awake()
		{
			animator = GetComponent<Animator>();
			playerController = GetComponentInParent<PlayerController>();
		}

		void Update()
		{
			animator.SetBool("IsMoving", playerController.IsMoving);
			if (playerController.IsMoving) {
				animator.SetFloat("Horizontal", playerController.Movement.x);
				animator.SetFloat("Vertical", playerController.Movement.y);
			}
		}
	}
}
