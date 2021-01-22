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
		RectTransform rectTransform;
		CanvasGroup canvasGroup;
		readonly Dictionary<UiAnimationTypes, Action> animationMap = new Dictionary<UiAnimationTypes, Action>();

		void Awake()
		{
			animationMap.Add(UiAnimationTypes.Fade, Fade);
			animationMap.Add(UiAnimationTypes.Move, Move);
			animationMap.Add(UiAnimationTypes.Scale, Scale);
			animationMap.Add(UiAnimationTypes.ScaleUpAndDown, ScaleUpAndDown);

			rectTransform = objectToAnimate.GetComponent<RectTransform>();
			canvasGroup = objectToAnimate.GetComponent<CanvasGroup>();
		}

		void Start()
		{
			if (playOnAwake)
				HandleTween();
		}

		public void HandleTween()
		{
			if (objectToAnimate == null)
				objectToAnimate = gameObject;

			LeanTween.reset();

			animationMap[animationType]();

			tweenObject.setDelay(delay);
			tweenObject.setEase(easeType);

			if (loop)
				tweenObject.loopCount = int.MaxValue;
			if (pingPong)
				tweenObject.setLoopPingPong();
		}

		void Fade()
		{
			if (startPositionOffset)
				canvasGroup.alpha = from.x;

			tweenObject = LeanTween.alphaCanvas(canvasGroup, to.x, duration);
		}

		void Move()
		{
			rectTransform.anchoredPosition = from;

			tweenObject = LeanTween.move(rectTransform, to, duration);
		}

		void Scale()
		{
			if (startPositionOffset)
				rectTransform.localScale = from;

			tweenObject = LeanTween.scale(objectToAnimate, to, duration);
		}

		void ScaleUpAndDown()
		{
			if (startPositionOffset)
				rectTransform.localScale = from;

			tweenObject = LeanTween.scale(objectToAnimate, to, duration * 0.5f)
				.setOnComplete(() => LeanTween.scale(objectToAnimate, from, duration * 0.5f));
		}
	}
}
