using UnityEngine;

namespace Fralle.Targeting
{
	public class TargetController : MonoBehaviour
	{
		[SerializeField] float hideDelay = 0.5f;
		[SerializeField] GameObject ui;

		float hideTimer;

		public void RaycastHit()
		{
			if (hideTimer < hideDelay)
				hideTimer = hideDelay;
			Show();
		}

		public void RaycastHit(float customHideDelay)
		{
			if (hideTimer < customHideDelay)
				hideTimer = customHideDelay;
			Show();
		}

		void Awake()
		{
			Hide();
		}

		void Update()
		{
			if (hideTimer < 0)
				return;
			hideTimer -= Time.deltaTime;

			if (hideTimer <= 0)
			{
				Hide();
			}
		}

		void Show()
		{
			ui.SetActive(true);
		}

		void Hide()
		{
			ui.SetActive(false);
		}

	}
}
