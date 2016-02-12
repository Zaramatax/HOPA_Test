using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace Framework {
    public enum AnimationType { Fade, Rotate, Move, Interactable }

    public class AnimationSystem : MonoBehaviour {

        public static AnimationSystem Instance;

        void Awake() {
            if (Instance != null) {
                DestroyImmediate(gameObject);
            } else {
                Instance = this;
            }
        }

        public void Restore(GameObject go, string state) {
            string[] animations = state.Split('/');

            foreach(string anim in animations) {
                if(anim != "") {
                    string[] animParams = anim.Split(';');

                    switch ((AnimationType)Enum.Parse(typeof(AnimationType), animParams[0])) {
                        case AnimationType.Fade:
                            Fade(go, Convert.ToSingle(animParams[1]), Convert.ToSingle(animParams[2]), Convert.ToSingle(animParams[3]), animParams[4], Convert.ToBoolean(animParams[5]));
                            break;

                        case AnimationType.Move:
                            Move(go, Utils.Vector2Parse(animParams[1]), Utils.Vector2Parse(animParams[2]), Convert.ToSingle(animParams[3]), animParams[4]);
                            break;
                    }
                }
            }
        }

        private AnimationUpdater SetComponents(GameObject go, AnimationType animType) {
            var animationUpdater = go.GetComponent<AnimationUpdater>();
            if (animationUpdater == null) {
                animationUpdater = go.AddComponent<AnimationUpdater>();
            }

            if(animType == AnimationType.Fade || animType == AnimationType.Interactable) {
                var canvasGroup = go.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                    canvasGroup = go.AddComponent<CanvasGroup>();
            }

            return animationUpdater;
        }


        //external functions
        public void Fade(GameObject go, float start, float end, float time, string functionName = "", bool inactiveAfterAnim = false) {
            var animationUpdater = SetComponents(go, AnimationType.Fade);
            var fade = new FadeAnimation(start, end, time, functionName, inactiveAfterAnim);
            animationUpdater.Play(fade);
        }

        public void Move(GameObject go, Vector2 start, Vector2 target, float speed, string functionName = "", bool inactiveAfterAnim = false) {
            var animationUpdater = SetComponents(go, AnimationType.Move);
            var move = new MoveAnimation(start, target, speed, functionName, inactiveAfterAnim);
            animationUpdater.Play(move);
        }

        public void Show(GameObject go, float time, string functionName = "") {
            var animationUpdater = SetComponents(go, AnimationType.Fade);
            var fade = new FadeAnimation(0, 1, time, functionName);
            go.SetActive(true);
            go.GetComponent<CanvasGroup>().interactable = true; 
            animationUpdater.Play(fade);
        }

        public void Hide(GameObject go, float time, string functionName = "") {
            var animationUpdater = SetComponents(go, AnimationType.Fade);
            var fade = new FadeAnimation(1, 0, time, functionName, true);
            go.GetComponent<CanvasGroup>().interactable = false;
            animationUpdater.Play(fade);
        }

        public void InstShow(GameObject go) {
            var animationUpdater = SetComponents(go, AnimationType.Fade);
            var fade = new FadeAnimation(0, 1, 0);
            go.SetActive(true);
            go.GetComponent<CanvasGroup>().interactable = true;
            animationUpdater.Play(fade);
        }

        public void InstHide(GameObject go) {
            var animationUpdater = SetComponents(go, AnimationType.Fade);
            var fade = new FadeAnimation(1, 0, 0);
            go.SetActive(false);
            go.GetComponent<CanvasGroup>().interactable = false;
            animationUpdater.Play(fade);
        }
        //external functions -----------///////////////
    }
}
