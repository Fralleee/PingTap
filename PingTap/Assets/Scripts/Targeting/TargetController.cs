using EPOOutline;
using UnityEngine;

namespace Fralle.Targeting
{
	public class TargetController : MonoBehaviour
	{
		[SerializeField] float hideDelay = 0.5f;
		[SerializeField] GameObject ui;

		Outlinable outlinable;
		float hideTimer;
		bool isShowing;

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
			outlinable = GetComponentInParent<Outlinable>();

			DelayedHide();
		}

		void Update()
		{
			if (hideTimer < 0)
				return;
			hideTimer -= Time.deltaTime;

			if (hideTimer <= 0)
			{
				DelayedHide();
			}
		}

		void FixedUpdate()
		{
			Hide();
		}

		void Show()
		{
			ui.SetActive(true);
			outlinable.enabled = true;
			isShowing = true;
		}

		void DelayedHide()
		{
			ui.SetActive(false);
		}

		void Hide()
		{
			if (isShowing)
			{
				isShowing = false;
				return;
			}
			outlinable.enabled = false;
		}

	}
}
