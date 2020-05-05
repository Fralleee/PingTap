using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Core.Animation
{
  public class UiTweener : MonoBehaviour
  {
    public GameObject objectToAnimate;

    public UiAnimationTypes animationType;
    public LeanTweenType easeType;

    public float duration;
    public float delay;

    public bool loop;
    public bool pingPong;

    public bool startPositionOffset;

    public Vector3 from;
    public Vector3 to;

    LtDescr tweenObject;
    readonly Dictionary<UiAnimationTypes, Action> animationMap = new Dictionary<UiAnimationTypes, Action>();

    void Awake()
    {
      animationMap.Add(UiAnimationTypes.Fade, Fade);
      animationMap.Add(UiAnimationTypes.Move, Move);
      animationMap.Add(UiAnimationTypes.Scale, Scale);
      animationMap.Add(UiAnimationTypes.ScaleUpAndDown, ScaleUpAndDown);
    }

    public void HandleTween()
    {
      if (objectToAnimate == null) objectToAnimate = gameObject;

      animationMap[animationType]();

      tweenObject.SetDelay(delay);
      tweenObject.SetEase(easeType);

      if (loop) tweenObject.loopCount = int.MaxValue;
      if (pingPong) tweenObject.SetLoopPingPong();
    }

    void Fade()
    {
      var objectCanvasGroup = objectToAnimate.GetComponent<CanvasGroup>();
      if (startPositionOffset) objectCanvasGroup.alpha = from.x;

      tweenObject = LeanTween.AlphaCanvas(objectCanvasGroup, to.x, duration);
    }

    void Move()
    {
      var rect = objectToAnimate.GetComponent<RectTransform>();
      rect.anchoredPosition = from;

      tweenObject = LeanTween.Move(rect, to, duration);
    }

    void Scale()
    {
      if (startPositionOffset) objectToAnimate.GetComponent<RectTransform>().localScale = from;

      tweenObject = LeanTween.Scale(objectToAnimate, to, duration);
    }

    void ScaleUpAndDown()
    {
      if (startPositionOffset) objectToAnimate.GetComponent<RectTransform>().localScale = from;

      float actualDuration = duration * 0.5f;
      tweenObject = LeanTween.Scale(objectToAnimate, to, actualDuration)
        .setOnComplete(() => LeanTween.Scale(objectToAnimate, from, actualDuration));
    }
  }
}