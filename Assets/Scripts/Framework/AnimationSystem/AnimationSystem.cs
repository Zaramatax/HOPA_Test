using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace Framework {
    public enum AnimationType { Fade, Rotate, Move, Enable }

    public class AnimationSystem : MonoBehaviour {

        public static AnimationSystem Instance;

        private List<AnimationSystemObject> objects;

        void Awake() {
            if (Instance != null) {
                DestroyImmediate(gameObject);
            } else {
                Instance = this;
            }

            objects = new List<AnimationSystemObject>();

            Load();
            objects.ForEach(x => PlayAnimation(x));
        }

        void OnApplicationQuit() {
            objects.ForEach(x => x.UpdateObjectsState());
            Save();
        }

        private void SetComponents(GameObject go, AnimationType animType) {
            var animationUpdater = go.GetComponent<AnimationUpdater>();
            if (animationUpdater == null)
                animationUpdater = go.AddComponent<AnimationUpdater>();

            animationUpdater.AnimationComplete += OnAnimationComplete;

            if(animType == AnimationType.Fade || animType == AnimationType.Enable) {
                var canvasGroup = go.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                    canvasGroup = go.AddComponent<CanvasGroup>();
            }
        }

        private void PlayAnimation(AnimationSystemObject animInfo) {
            switch (animInfo.type) {
                case AnimationType.Fade:
                    var fadeState = (FadeAnimationState)animInfo.state;
                    Fade(animInfo.animatedObject, fadeState.currentValue, fadeState.endValue, fadeState.timeLeft, animInfo.functionName);
                    break;

                case AnimationType.Move:
                    var moveState = (MoveAnimationState)animInfo.state;
                    Move(animInfo.animatedObject, moveState.currentPos, moveState.targetPos, moveState.speed, animInfo.functionName);
                    break;
            }
        }

        public void OnAnimationComplete(GameObject go, AnimationType type) {
            go.GetComponent<AnimationUpdater>().AnimationComplete -= OnAnimationComplete;

            var animSystObj = objects.Find(x => (x.animatedObject == go) && (x.type == type));
            if(animSystObj.functionName != "")
                go.SendMessageUpwards(animSystObj.functionName);
            objects.Remove(animSystObj);
        }

        private void Save() {
            XmlDocument doc = new XmlDocument();
            XmlNode root = doc.CreateElement("root");
            doc.AppendChild(root);

            objects.ForEach(x => root.AppendChild(x.Save(doc)));

            ProfileSaver.Save(doc, "animation_system.xml");
        }

        private void Load() {
            XmlDocument doc = new XmlDocument();
            ProfileSaver.Load(doc, "animation_system.xml");
            XmlElement root = doc.DocumentElement;
            if (root == null) return;

            foreach (XmlElement objElement in root) {
                var currentObj = new AnimationSystemObject();
                currentObj.Load(objElement);
                objects.Add(currentObj);
            }
        }


        //external functions
        public void Fade(GameObject go, float startValue, float endValue, float time, string functionName = "") {
            SetComponents(go, AnimationType.Fade);
            go.GetComponent<AnimationUpdater>().Fade(startValue, endValue, time);

            var sameAnimation = objects.Find(x => (x.animatedObject == go) && (x.type == AnimationType.Fade));
            if (sameAnimation != null) {
                objects.Remove(sameAnimation);
            }

            objects.Add(new AnimationSystemObject { animatedObject = go, functionName = functionName, type = AnimationType.Fade });
        }

        public void Move(GameObject go, Vector2 startPos, Vector2 targetPos, float speed, string functionName = "") {
            SetComponents(go, AnimationType.Move);
            go.GetComponent<AnimationUpdater>().Move(startPos, targetPos, speed);

            var sameAnimation = objects.Find(x => (x.animatedObject == go) && (x.type == AnimationType.Move));
            if (sameAnimation != null) {
                objects.Remove(sameAnimation);
            }

            objects.Add(new AnimationSystemObject { animatedObject = go, functionName = functionName, type = AnimationType.Move });
        }
    }

    public class AnimationSystemObject {
        public GameObject animatedObject;
        public string functionName;
        public AnimationType type;

        public object state;

        public void UpdateObjectsState() {
            switch (type) {
                case AnimationType.Fade:
                    state = animatedObject.GetComponent<AnimationUpdater>().fadeAnimationState;
                    break;

                case AnimationType.Move:
                    state = animatedObject.GetComponent<AnimationUpdater>().moveAnimationState;
                    break;
            }
        }

        public XmlNode Save(XmlDocument doc) {

            XmlElement animation_object = doc.CreateElement(Enum.GetName(typeof(AnimationType), (int)type));

            animation_object.SetAttribute("animated_obj", Convert.ToString(Utils.GetGameObjectFullName(animatedObject)));
            animation_object.SetAttribute("func_name", functionName);

            switch (type) {
                case AnimationType.Fade:
                    var fadeState = (FadeAnimationState)state;

                    animation_object.SetAttribute("current_value", Convert.ToString(fadeState.currentValue));
                    animation_object.SetAttribute("end_value", Convert.ToString(fadeState.endValue));
                    animation_object.SetAttribute("time_left", Convert.ToString(fadeState.timeLeft));
                    break;

                case AnimationType.Move:
                    var moveState = (MoveAnimationState)state;

                    animation_object.SetAttribute("current_pos", Convert.ToString(moveState.currentPos));
                    animation_object.SetAttribute("target_pos", Convert.ToString(moveState.targetPos));
                    animation_object.SetAttribute("speed", Convert.ToString(moveState.speed));
                    break;
            }

            return animation_object;
        }

        public void Load(XmlElement objInfo) {
            if (objInfo == null) return;

            animatedObject = GameObject.Find(objInfo.GetAttribute("animated_obj"));
            functionName = objInfo.GetAttribute("func_name");
            type = (AnimationType)Enum.Parse(typeof(AnimationType), objInfo.Name);

            switch (type) {
                case AnimationType.Fade:
                    var fadeState = new FadeAnimationState();
                    fadeState.currentValue = Convert.ToSingle(objInfo.GetAttribute("current_value"));
                    fadeState.endValue = Convert.ToSingle(objInfo.GetAttribute("end_value"));
                    fadeState.timeLeft = Convert.ToSingle(objInfo.GetAttribute("time_left"));
                    state = fadeState;
                    break;

                case AnimationType.Move:
                    var moveState = new MoveAnimationState();
                    moveState.currentPos = Utils.Vector2Parse(objInfo.GetAttribute("current_pos"));
                    moveState.targetPos = Utils.Vector2Parse(objInfo.GetAttribute("target_pos"));
                    moveState.speed = Convert.ToSingle(objInfo.GetAttribute("speed"));
                    state = moveState;
                    break;
            }
        }
    }
}
