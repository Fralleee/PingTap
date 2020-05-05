#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AnimationRecorder : MonoBehaviour
{
  const float CapturingInterval = 1.0f / 30.0f;

  float lastCapturedTime;
  float recordingTimer;
  bool recording;

  List<AnimationRecorderItem> recorders;

	void Start()
	{
		Configurate();
	}

	void Configurate()
	{
		recorders = new List<AnimationRecorderItem>();
		recordingTimer = 0.0f;

		var allTransforms = gameObject.GetComponentsInChildren<Transform>();
		for (int i = 0; i < allTransforms.Length; ++i)
		{
			string path = CreateRelativePathForObject(transform, allTransforms[i]);
			recorders.Add(new AnimationRecorderItem(path, allTransforms[i]));
		}
	}

	public void StartRecording()
	{
		Debug.Log("AnimationRecorder recording started");
		recording = true;
	}

	public void StopRecording()
	{
		Debug.Log("AnimationRecorder recording stopped");
		recording = false;
		ExportAnimationClip();
		Configurate();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && !recording)
		{
			StartRecording();
			return;
		}

		if (Input.GetKeyDown(KeyCode.Space) && recording)
		{
			StopRecording();
			return;
		}

		if (recording)
		{
			if (recordingTimer == 0.0f || recordingTimer - lastCapturedTime >= CapturingInterval)
			{
				for (int i = 0; i < recorders.Count; ++i)
				{
					recorders[i].AddFrame(recordingTimer);
				}
				lastCapturedTime = recordingTimer;
			}
			recordingTimer += Time.deltaTime;
		}
	}

  void ExportAnimationClip()
	{
		AnimationClip clip = new AnimationClip();
		for (int i = 0; i < recorders.Count; ++i)
		{
			Dictionary<string, AnimationCurve> propertiles = recorders[i].Properties;
			for (int j = 0; j < propertiles.Count; ++j)
			{
				string name = recorders[i].PropertyName;
				string propery = propertiles.ElementAt(j).Key;
				var curve = propertiles.ElementAt(j).Value;
				clip.SetCurve(name, typeof(Transform), propery, curve);
			}
		}
		clip.EnsureQuaternionContinuity();

		string path = "Assets/" + gameObject.name + ".anim";
		AssetDatabase.CreateAsset(clip, path);
		Debug.Log("AnimationRecorder saved to = " + path);
	}

  string CreateRelativePathForObject(Transform root, Transform target)
	{
		if (target == root)
		{
			return string.Empty;
		}

		string name = target.name;
		Transform bufferTransform = target;

		while (bufferTransform.parent != root)
		{
			name = string.Format("{0}/{1}", bufferTransform.parent.name, name);
			bufferTransform = bufferTransform.parent;
		}
		return name;
	}
}
#endif