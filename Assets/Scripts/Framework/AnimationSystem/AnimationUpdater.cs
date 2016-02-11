using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System;

namespace Framework {
    public class AnimationUpdater : MonoBehaviour {

        public event Action<GameObject, AnimationType> AnimationComplete;

        private IEnumerator fadeCoroutine;
        public FadeAnimationState fadeAnimationState { get; private set; }

        private IEnumerator moveCoroutine;
        public MoveAnimationState moveAnimationState { get; private set; }

        public void Awake() {
            fadeAnimationState = new FadeAnimationState();
            moveAnimationState = new MoveAnimationState();
        }



       
        /////////////////////////////////
        /// ------===== FADE =====------
        /////////////////////////////////

        public void Fade(float startValue, float endValue, float time) {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = FadeCoroutine(startValue, endValue, time);
            StartCoroutine(fadeCoroutine);
        }

        IEnumerator FadeCoroutine(float startValue, float endValue, float time) {
            var canvasGroup = GetComponent<CanvasGroup>();
            float delta = endValue - startValue;

            canvasGroup.alpha = startValue;
            while (Math.Abs(canvasGroup.alpha - endValue) > 0.05f) {
                canvasGroup.alpha += (delta / time) * Time.deltaTime;

                fadeAnimationState.currentValue = canvasGroup.alpha;
                fadeAnimationState.endValue = endValue;
                fadeAnimationState.timeLeft = ((endValue - canvasGroup.alpha) / endValue) * time;

                yield return new WaitForEndOfFrame();
            }
            canvasGroup.alpha = endValue;

            if (AnimationComplete != null)
                AnimationComplete(gameObject, AnimationType.Fade);
        }



        /////////////////////////////////
        /// ------===== MOVE =====------
        /////////////////////////////////

        public void Move(Vector2 startPos, Vector2 targetPos, float speed) {
            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);

            moveCoroutine = MoveCoroutine(startPos, targetPos, speed);
            StartCoroutine(moveCoroutine);
        }

        IEnumerator MoveCoroutine(Vector2 startPos, Vector2 targetPos, float speed) {
            transform.position = startPos;

            var fullDist = Vector2.Distance(transform.position, targetPos);

            var t = 0f;

            while (t < 1) {

                t += (fullDist / (fullDist / speed)) / fullDist;

                transform.position = Vector2.Lerp(startPos, targetPos, t);

                moveAnimationState.currentPos = transform.position;
                moveAnimationState.targetPos = targetPos;
                moveAnimationState.speed = speed;

                yield return new WaitForEndOfFrame();
            }

            transform.position = targetPos;

            if (AnimationComplete != null)
                AnimationComplete(gameObject, AnimationType.Move);
        }
    }

    public class FadeAnimationState : MonoBehaviour {
        public float currentValue;
        public float endValue;
        public float timeLeft;
    }

    public class MoveAnimationState : MonoBehaviour {
        public Vector2 currentPos;
        public Vector2 targetPos;
        public float speed;
    }
}
