using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationRecorderItem
{
  const float MinDiffDistance = 0.025f;
  const float MinDiffScale = 0.025f;
  const float MinDiffAngle = 5.0f;

	public Dictionary<string, AnimationCurve> Properties { get; }

	public string PropertyName { get; }

  Transform animationObject;
  Vector3? lastPosition;
  Vector3? lastScale;
  Quaternion? lastRotation;

	public AnimationRecorderItem(string propertyName, Transform animatingObject)
	{
		Properties = new Dictionary<string, AnimationCurve>();
		PropertyName = propertyName;
		animationObject = animatingObject;

		Properties.Add("localPosition.x", new AnimationCurve());
		Properties.Add("localPosition.y", new AnimationCurve());
		Properties.Add("localPosition.z", new AnimationCurve());

		Properties.Add("localRotation.x", new AnimationCurve());
		Properties.Add("localRotation.y", new AnimationCurve());
		Properties.Add("localRotation.z", new AnimationCurve());
		Properties.Add("localRotation.w", new AnimationCurve());

		Properties.Add("localScale.x", new AnimationCurve());
		Properties.Add("localScale.y", new AnimationCurve());
		Properties.Add("localScale.z", new AnimationCurve());
	}

	public void AddFrame(float time)
	{
		if (lastPosition == null || Vector3.Distance((Vector3)lastPosition, animationObject.localPosition) > MinDiffDistance)
		{
			Properties["localPosition.x"].AddKey(new Keyframe(time, animationObject.localPosition.x, 0.0f, 0.0f));
			Properties["localPosition.y"].AddKey(new Keyframe(time, animationObject.localPosition.y, 0.0f, 0.0f));
			Properties["localPosition.z"].AddKey(new Keyframe(time, animationObject.localPosition.z, 0.0f, 0.0f));
			lastPosition = animationObject.localPosition;
		}

		if (lastRotation == null || Quaternion.Angle((Quaternion)lastRotation, animationObject.localRotation) > MinDiffAngle)
		{
			Properties["localRotation.x"].AddKey(new Keyframe(time, animationObject.localRotation.x, 0.0f, 0.0f));
			Properties["localRotation.y"].AddKey(new Keyframe(time, animationObject.localRotation.y, 0.0f, 0.0f));
			Properties["localRotation.z"].AddKey(new Keyframe(time, animationObject.localRotation.z, 0.0f, 0.0f));
			Properties["localRotation.w"].AddKey(new Keyframe(time, animationObject.localRotation.w, 0.0f, 0.0f));
			lastRotation = animationObject.localRotation;
		}

		if (lastScale == null || Vector3.Distance((Vector3)lastScale, animationObject.localScale) > MinDiffScale)
		{
			Properties["localScale.x"].AddKey(new Keyframe(time, animationObject.localScale.x, 0.0f, 0.0f));
			Properties["localScale.y"].AddKey(new Keyframe(time, animationObject.localScale.y, 0.0f, 0.0f));
			Properties["localScale.z"].AddKey(new Keyframe(time, animationObject.localScale.z, 0.0f, 0.0f));
			lastScale = animationObject.localScale;
		}
	}
}