using CombatSystem;
using Fralle.Core.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Fralle.UI.HUD
{
	public class HitmarkersUI : MonoBehaviour
	{
		[SerializeField] float fadeTimer = 0.5f;

		[SerializeField] Color majorColor;
		[SerializeField] Color nerveColor;

		[SerializeField] int defaultSize = 32;
		[SerializeField] int maxSize = 96;

		AudioSource audioSource;
		RectTransform rectTransform;
		Image image;
		Color currentColor;
		float lastHit;

		void Awake()
		{
			audioSource = GetComponent<AudioSource>();
			rectTransform = GetComponent<RectTransform>();
			image = GetComponent<Image>();
			Combatant combatant = GetComponentInParent<Combatant>();
			combatant.OnHit += Combatant_OnHit;
		}

		void Update()
		{
			if (lastHit > Time.time)
				Fade();
		}

		void Combatant_OnHit(DamageData obj)
		{
			ActivateHitmarker(obj.HitArea);
		}

		void Fade()
		{
			float percentage = 1 - ((lastHit - Time.time) / fadeTimer);

			image.color = Color.Lerp(currentColor.Alpha(0.8f), currentColor.Alpha(0f), percentage);

			float newSize = Mathf.Lerp(rectTransform.sizeDelta.x, defaultSize, percentage);
			rectTransform.sizeDelta = new Vector2(
				Mathf.Clamp(newSize, defaultSize, maxSize),
				Mathf.Clamp(newSize, defaultSize, maxSize)
			);
		}

		void AdjustSize(int amount)
		{
			if (rectTransform.sizeDelta.magnitude < maxSize)
			{
				rectTransform.sizeDelta = new Vector2(
					Mathf.Clamp(rectTransform.sizeDelta.x + amount, defaultSize, maxSize),
					Mathf.Clamp(rectTransform.sizeDelta.y + amount, defaultSize, maxSize)
				);
			}
		}

		void ActivateHitmarker(HitArea hitArea)
		{
			lastHit = Time.time + fadeTimer;
			if (hitArea == HitArea.Chest)
			{
				currentColor = majorColor.Alpha(0.8f);
				audioSource.pitch = 1;
				AdjustSize(8);
			}
			else
			{
				currentColor = nerveColor.Alpha(0.8f);
				audioSource.pitch = 3;
				AdjustSize(16);
			}
			//audioSource.Play();
		}
	}
}
