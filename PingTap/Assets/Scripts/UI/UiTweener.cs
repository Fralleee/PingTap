using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.UI
{
  public class UiTweener : MonoBehaviour
  {
    public GameObject objectToAnimate;

    public UiAnimationTypes animationType;
    public LeanTweenType easeType;

    public float duration;
    public float delay;

    public bool playOnAwake;
    public bool loop;
    public bool pingPong;

    public bool startPositionOffset;

    public Vector3 from;
    public Vector3 to;

    LTDescr tweenObject;
    readonly Dictionary<UiAnimationTypes, Action> animationMap = new Dictionary<UiAnimationTypes, Action>();

    void Awake()
    {
      animationMap.Add(UiAnimationTypes.Fade, Fade);
      animationMap.Add(UiAnimationTypes.Move, Move);
      animationMap.Add(UiAnimationTypes.Scale, Scale);
      animationMap.Add(UiAnimationTypes.ScaleUpAndDown, ScaleUpAndDown);
    }

    void Start()
    {
      if (playOnAwake) HandleTween();
    }

    public void HandleTween()
    {
      if (objectToAnimate == null) objectToAnimate = gameObject;

      animationMap[animationType]();

      tweenObject.setDelay(delay);
      tweenObject.setEase(easeType);

      if (loop) tweenObject.loopCount = int.MaxValue;
      if (pingPong) tweenObject.setLoopPingPong();
    }

    void Fade()
    {
      var objectCanvasGroup = objectToAnimate.GetComponent<CanvasGroup>();
      if (startPositionOffset) objectCanvasGroup.alpha = from.x;

      tweenObject = LeanTween.alphaCanvas(objectCanvasGroup, to.x, duration);
    }

    void Move()
    {
      var rect = objectToAnimate.GetComponent<RectTransform>();
      rect.anchoredPosition = from;

      tweenObject = LeanTween.move(rect, to, duration);
    }

    void Scale()
    {
      if (startPositionOffset) objectToAnimate.GetComponent<RectTransform>().localScale = from;

      tweenObject = LeanTween.scale(objectToAnimate, to, duration);
    }

    void ScaleUpAndDown()
    {
      if (startPositionOffset) objectToAnimate.GetComponent<RectTransform>().localScale = from;

      float actualDuration = duration * 0.5f;
      tweenObject = LeanTween.scale(objectToAnimate, to, actualDuration)
        .setOnComplete(() => LeanTween.scale(objectToAnimate, from, actualDuration));
    }
  }
}