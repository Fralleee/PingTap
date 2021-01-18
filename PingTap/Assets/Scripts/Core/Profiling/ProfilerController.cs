using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

namespace Fralle.Core.Profiling
{
	public class ProfilerController : MonoBehaviour
	{
		ProfilerRecorder systemMemoryRecorder;
		ProfilerRecorder gcMemoryRecorder;
		ProfilerRecorder mainThreadTimeRecorder;
		ProfilerRecorder drawCallsCountRecorder;

		public float frameTime;
		public float fps;
		public float gcMemory;
		public float systemMemory;
		public float drawCalls;

		static double GetRecorderFrameAverage(ProfilerRecorder recorder)
		{
			var samplesCount = recorder.Capacity;
			if (samplesCount == 0)
				return 0;

			double r = 0;
			var samples = new List<ProfilerRecorderSample>(samplesCount);
			recorder.CopyTo(samples);
			for (var i = 0; i < samples.Count; ++i)
				r += samples[i].Value;
			r /= samplesCount;

			return r;
		}

		void OnEnable()
		{
			systemMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
			gcMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
			mainThreadTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "Main Thread", 15);
			drawCallsCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
		}

		void OnDisable()
		{
			systemMemoryRecorder.Dispose();
			gcMemoryRecorder.Dispose();
			mainThreadTimeRecorder.Dispose();
			drawCallsCountRecorder.Dispose();
		}

		void Update()
		{
			frameTime = (float)GetRecorderFrameAverage(mainThreadTimeRecorder) * (1e-6f);
			fps = 1000 / frameTime;
			gcMemory = gcMemoryRecorder.LastValue / (1024f * 1024f);
			systemMemory = systemMemoryRecorder.LastValue / (1024f * 1024f);
			drawCalls = drawCallsCountRecorder.LastValue;
		}
	}
}
