using UnityEngine;

namespace Fralle.Core.Animation
{
	public class FadeoutLineRenderer : MonoBehaviour
	{
		public float fadeoutTime;

		float fadeTimer;
		LineRenderer lineRenderer;
		Color currentColor;

		void Awake()
		{
			lineRenderer = GetComponent<LineRenderer>();
			currentColor = lineRenderer.material.color;
			fadeTimer = fadeoutTime;
		}

		void OnEnable()
		{
			fadeTimer = fadeoutTime;
		}

		void Update()
		{
			fadeTimer -= Time.deltaTime;

			var newColor = Color.Lerp(currentColor, new Color(1f, 1f, 1f, 0f), 1 - (fadeTimer / fadeoutTime));
			lineRenderer.material.color = newColor;
		}
	}
}
