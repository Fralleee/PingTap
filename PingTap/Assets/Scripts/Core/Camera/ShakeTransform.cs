using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Core.Camera
{
  public class ShakeTransform : MonoBehaviour
  {
    [System.Serializable]
    public class ShakeEvent
    {
      readonly float duration;
      float timeRemaining;
      readonly ShakeTransformEventData data;
      Vector3 noiseOffset;
      public Vector3 noise;

      public ShakeTransformEventData.Target Target => data.target;

      public bool IsAlive()
      {
        return timeRemaining > 0.0f;
      }


      public ShakeEvent(ShakeTransformEventData data)
      {
        this.data = data;

        duration = data.duration;
        timeRemaining = duration;

        const float rand = 32.0f;

        noiseOffset.x = Random.Range(0.0f, rand);
        noiseOffset.y = Random.Range(0.0f, rand);
        noiseOffset.z = Random.Range(0.0f, rand);
      }

      public void Update()
      {
        var deltaTime = Time.deltaTime;

        timeRemaining -= deltaTime;

        var noiseOffsetDelta = deltaTime * data.frequency;

        noiseOffset.x += noiseOffsetDelta;
        noiseOffset.y += noiseOffsetDelta;
        noiseOffset.z += noiseOffsetDelta;

        noise.x = Mathf.PerlinNoise(noiseOffset.x, 0.0f);
        noise.y = Mathf.PerlinNoise(noiseOffset.y, 1.0f);
        noise.z = Mathf.PerlinNoise(noiseOffset.z, 2.0f);

        noise -= Vector3.one * 0.5f;

        noise *= data.amplitude;

        var agePercent = 1.0f - (timeRemaining / duration);
        noise *= data.blendOverLifetime.Evaluate(agePercent);
      }
    }

    readonly List<ShakeEvent> shakeEvents = new List<ShakeEvent>();

    public void AddShakeEvent(ShakeTransformEventData data)
    {
      shakeEvents.Add(new ShakeEvent(data));
    }

    public void AddShakeEvent(float amplitude, float frequency, float duration, AnimationCurve blendOverLifetime,
      ShakeTransformEventData.Target target)
    {
      var data = ScriptableObject.CreateInstance<ShakeTransformEventData>();
      data.Init(amplitude, frequency, duration, blendOverLifetime, target);

      AddShakeEvent(data);
    }

    void LateUpdate()
    {
      var positionOffset = Vector3.zero;
      var rotationOffset = Vector3.zero;

      for (var i = shakeEvents.Count - 1; i != -1; i--)
      {
        var se = shakeEvents[i];
        se.Update();

        if (se.Target == ShakeTransformEventData.Target.Position)
        {
          positionOffset += se.noise;
        }
        else
        {
          rotationOffset += se.noise;
        }

        if (!se.IsAlive())
        {
          shakeEvents.RemoveAt(i);
        }
      }

      transform.localPosition = positionOffset;
      transform.localEulerAngles = rotationOffset;
    }
  }
}