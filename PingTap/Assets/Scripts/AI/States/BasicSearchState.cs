using Fralle.FpsController;
using UnityEngine;

namespace Fralle.PingTap.AI
{
	[CreateAssetMenu(menuName = "AI/States/Search/Basic")]
	public class BasicSearchState : SearchState
	{
		AIBrain aiBrain;
		AIController controller;

		float searchDistance = 10f;

		public override void OnEnter()
		{
			controller.speed = controller.runSpeed;
		}

		public override void OnLogic()
		{
			if (controller.remainingDistance > 0.5f)
				return;

			controller.SetRandomDestination(searchDistance);
		}

		public override void OnExit()
		{
			controller.Stop(controller.walkSpeed);
		}

		public override void Setup(AIBrain aiBrain)
		{
			this.aiBrain = aiBrain;
			controller = aiBrain.GetComponent<AIController>();
		}
	}
}
