using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Fralle.PingTap.Benchmark
{
  public class BenchmarkController : MonoBehaviour
  {
    [SerializeField] BenchmarkEvent benchmarkEvent;
    [SerializeField] TextMeshProUGUI textMeshProUgui;

    int frameCount = 0;
    float timePassed = 0f;

    void Start()
    {
      if (benchmarkEvent)
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
        float fps = frameCount / timePassed;
        textMeshProUgui.text = fps.ToString();

        frameCount = 0;
        timePassed = 0;
        return fps;
      }
    }

    public void RunEnumerators(IEnumerator[] enumerators)
    {
      StartCoroutine(RunEnumeratorsCoroutine(enumerators));
    }

    IEnumerator RunEnumeratorsCoroutine(IEnumerable<IEnumerator> enumerators) => enumerators.Select(StartCoroutine).GetEnumerator();
  }
}
