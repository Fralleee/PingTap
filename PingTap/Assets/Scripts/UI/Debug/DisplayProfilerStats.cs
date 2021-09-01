using Fralle.Core.Profiling;
using System.Text;
using TMPro;
using UnityEngine;

namespace Fralle.UI
{
  public class DisplayProfilerStats : MonoBehaviour
  {
    ProfilerController profilerController;
    TextMeshProUGUI statsText;

    static bool debugStats = false;
    static bool frameTime = false;
    static bool fps = true;
    static bool gcMemory = false;
    static bool systemMemory = false;
    static bool drawCalls = false;

    void Awake()
    {
      profilerController = GetComponent<ProfilerController>();
      statsText = GetComponent<TextMeshProUGUI>();
      ToggleProfiler(debugStats);
    }

    void Update()
    {
      if (!debugStats)
        return;

      StringBuilder sb = new StringBuilder(500);

      if (frameTime)
        sb.AppendLine($"Frame Time: {profilerController.frameTime:F1} ms");
      if (fps)
        sb.AppendLine($"FPS: {profilerController.fps:F1} ms");
      if (gcMemory)
        sb.AppendLine($"GC Memory: {profilerController.gcMemory} MB");
      if (systemMemory)
        sb.AppendLine($"System Memory: {profilerController.systemMemory} MB");
      if (drawCalls)
        sb.AppendLine($"Draw Calls: {profilerController.drawCalls}");

      statsText.text = sb.ToString();
    }

    public void ToggleProfiler(bool enableProfiler)
    {
      profilerController.enabled = enableProfiler;
      statsText.enabled = enableProfiler;
    }

    #region Commands
    //[Command(aliasOverride: "display_performance", description: "Display performance stats")]
    //public static void DisplayDebugStats(int enabled)
    //{
    //	debugStats = Convert.ToBoolean(enabled);
    //	FindObjectOfType<DisplayProfilerStats>().ToggleProfiler(debugStats);
    //}

    //[Command(aliasOverride: "performance_frame_time", description: "Show frame time")]
    //public static void ShowFrameTime(int enabled)
    //{
    //	frameTime = Convert.ToBoolean(enabled);
    //}

    //[Command(aliasOverride: "performance_fps", description: "Show fps count")]
    //public static void ShowFps(int enabled)
    //{
    //	fps = Convert.ToBoolean(enabled);
    //}

    //[Command(aliasOverride: "performance_gc_memory", description: "Show gc memory")]
    //public static void ShowGcMemory(int enabled)
    //{
    //	gcMemory = Convert.ToBoolean(enabled);
    //}

    //[Command(aliasOverride: "performance_system_memory", description: "Show system memory")]
    //public static void ShowSystemMemory(int enabled)
    //{
    //	systemMemory = Convert.ToBoolean(enabled);
    //}

    //[Command(aliasOverride: "performance_draw_calls", description: "Show draw calls")]
    //public static void ShowDrawCalls(int enabled)
    //{
    //	drawCalls = Convert.ToBoolean(enabled);
    //}
    #endregion
  }
}
