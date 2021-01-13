using TMPro;
using UnityEngine;

namespace Fralle.UI
{
	public class FPSCounter : MonoBehaviour
	{
		[SerializeField] float updateRate = 4f;

		TextMeshProUGUI text;

		int frameCount;
		float deltaTime;
		float calculatedUpdateRate;

		void Awake()
		{
			text = GetComponent<TextMeshProUGUI>();
			calculatedUpdateRate = 1 / updateRate;
		}

		void Update()
		{
			frameCount++;
			deltaTime += Time.deltaTime;

			if (deltaTime > calculatedUpdateRate)
			{
				text.text = $"{Mathf.RoundToInt(frameCount / deltaTime)} FPS";
				frameCount = 0;
				deltaTime -= calculatedUpdateRate;
			}
		}

		void OnValidate()
		{
			calculatedUpdateRate = 1 / updateRate;
		}
	}
}
