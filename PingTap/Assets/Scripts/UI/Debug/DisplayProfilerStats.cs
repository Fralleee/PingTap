using Fralle.Core.Profiling;
using QFSW.QC;
using System;
using System.Text;
using TMPro;
using UnityEngine;

namespace Fralle.UI
{
	public class DisplayProfilerStats : MonoBehaviour
	{
		ProfilerController profilerController;
		TextMeshProUGUI statsText;

		static bool showDebugStats = true;
		static bool showFrameTime = false;
		static bool showFps = true;
		static bool showGcMemory = false;
		static bool showSystemMemory = false;
		static bool showDrawCalls = false;

		void Awake()
		{
			profilerController = GetComponent<ProfilerController>();
			statsText = GetComponent<TextMeshProUGUI>();
		}

		void Update()
		{
			if (!showDebugStats)
			{
				statsText.enabled = false;
				return;
			}

			var sb = new StringBuilder(500);

			if (showFrameTime)
				sb.AppendLine($"Frame Time: {profilerController.frameTime:F1} ms");
			if (showFps)
				sb.AppendLine($"FPS: {profilerController.fps:F1} ms");
			if (showGcMemory)
				sb.AppendLine($"GC Memory: {profilerController.gcMemory} MB");
			if (showSystemMemory)
				sb.AppendLine($"System Memory: {profilerController.systemMemory} MB");
			if (showDrawCalls)
				sb.AppendLine($"Draw Calls: {profilerController.drawCalls}");

			statsText.text = sb.ToString();
		}

		[Command(aliasOverride: "show_debug_stats", description: "Show debug stats")]
		public static void ShowDebugStats(int enabled)
		{
			showDebugStats = Convert.ToBoolean(enabled);
		}

		[Command(aliasOverride: "show_frame_time", description: "Show frame time")]
		public static void ShowFrameTime(int enabled)
		{
			showFrameTime = Convert.ToBoolean(enabled);
		}

		[Command(aliasOverride: "show_fps", description: "Show fps count")]
		public static void ShowFps(int enabled)
		{
			showFps = Convert.ToBoolean(enabled);
		}

		[Command(aliasOverride: "show_gc_memory", description: "Show gc memory")]
		public static void ShowGcMemory(int enabled)
		{
			showGcMemory = Convert.ToBoolean(enabled);
		}

		[Command(aliasOverride: "show_system_memory", description: "Show system memory")]
		public static void ShowSystemMemory(int enabled)
		{
			showSystemMemory = Convert.ToBoolean(enabled);
		}

		[Command(aliasOverride: "show_draw_calls", description: "Show draw calls")]
		public static void ShowDrawCalls(int enabled)
		{
			showDrawCalls = Convert.ToBoolean(enabled);
		}

	}
}
