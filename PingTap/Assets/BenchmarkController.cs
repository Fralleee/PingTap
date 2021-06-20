using System.Collections;
using TMPro;
using UnityEngine;

public class BenchmarkController : MonoBehaviour
{
	[SerializeField] BenchmarkEvent benchmarkEvent;
	[SerializeField] TextMeshProUGUI textMeshProUGUI;

	int frameCount = 0;
	float timePassed = 0f;

	void Start()
	{
		if (benchmarkEvent != null)
			benchmarkEvent.Run(this);
	}

	void Update()
	{
		frameCount++;
		timePassed += Time.deltaTime;
	}

	public float CurrentFps
	{
		get
		{
			var fps = frameCount / timePassed;
			textMeshProUGUI.text = fps.ToString();

			frameCount = 0;
			timePassed = 0;
			return fps;
		}
	}

	public void RunEnumerators(IEnumerator[] enumerators)
	{
		StartCoroutine(RunEnumeratorsCoroutine(enumerators));
	}

	IEnumerator RunEnumeratorsCoroutine(IEnumerator[] enumerators)
	{
		foreach (var e in enumerators)
			yield return StartCoroutine(e);
	}
}
