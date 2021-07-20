using Fralle.FpsController;
using UnityEngine;

namespace Fralle
{
	public class PlayerAnimator : MonoBehaviour
	{
		Animator animator;
		PlayerController playerController;

		int animIsMoving;
		int animIsJumping;
		int animHorizontal;
		int animVertical;

		void Awake()
		{
			animator = GetComponent<Animator>();
			playerController = GetComponentInParent<PlayerController>();

			animIsMoving = Animator.StringToHash("IsMoving");
			animIsJumping = Animator.StringToHash("IsJumping");
			animHorizontal = Animator.StringToHash("Horizontal");
			animVertical = Animator.StringToHash("Vertical");
		}

		void Update()
		{
			animator.SetBool(animIsMoving, playerController.IsMoving);
			animator.SetBool(animIsJumping, playerController.IsJumping);
			if (playerController.IsMoving)
			{
				animator.SetFloat(animHorizontal, playerController.Movement.x);
				animator.SetFloat(animVertical, playerController.Movement.y);
			}
		}
	}
}
