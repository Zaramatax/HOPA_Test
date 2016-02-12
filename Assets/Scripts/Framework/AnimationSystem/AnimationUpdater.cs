using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System;

namespace Framework {
    public class AnimationUpdater : MonoBehaviour {

        public List<BaseAnimation> animations;

        void Awake() {
            animations = new List<BaseAnimation>();
        }

        void Update() {
            animations.ForEach(x => x.Update());
        }

        public string GetStateToString() {
            string state = "";

            for (int i = 0; i < animations.Count; i++) {
                var animState = animations[i].GetStateToString();

                if (animState == "") continue;

                state = state + animState;

                if(i != animations.Count - 1) {
                    state = state + "/";
                }
            }

            return state;
        }

        public void Play(BaseAnimation animation) {
            var sameAnimation = animations.Find(x => x._type == animation._type);
            if(sameAnimation != null) {
                sameAnimation.Complete -= OnAnimationComplete;
                animations.Remove(sameAnimation);
            }

            animations.Add(animation);
            animation.Complete += OnAnimationComplete;
            animation.Play(gameObject);
        }

        public void OnAnimationComplete(BaseAnimation anim) {
            anim.Complete -= OnAnimationComplete;
            animations.Remove(anim);
        }
    }


    //////////////////////////////////////////
    ///
    ///    All Base Animations
    ///
    //////////////////////////////////////////

    public abstract class BaseAnimation {
        public AnimationType _type { get; private set; }

        public event Action<BaseAnimation> Complete;

        protected string _funcName;
        protected bool _inactiveAfterAnim;
        protected GameObject _go;

        public BaseAnimation(AnimationType type, string funcName, bool inactiveAfterAnim) {
            _funcName = funcName;
            _inactiveAfterAnim = inactiveAfterAnim;
            _type = type;
        }

        public virtual void Play(GameObject go) { }

        public virtual void Update() { }

        public virtual string GetStateToString() {
            return "";
        }

        public void SendComplete(BaseAnimation anim) {
            if (Complete != null) {
                Complete(anim);
            }
        }
    }


    //////////////////////////////////////////
    ///
    ///    ---======= FADE ======-----
    ///
    //////////////////////////////////////////
    public class FadeAnimation : BaseAnimation {
        private float _start;
        private float _end;
        private float _time;

        private CanvasGroup _canvasGroup;

        private bool _animFlag;

        public FadeAnimation(float start, float end, float time, string funcName = "", bool inactiveAfterAnim = false) : base(AnimationType.Fade, funcName, inactiveAfterAnim) {
            _start = start;
            _end = end;
            _time = time;
        }

        public override void Play(GameObject go) {
            _go = go;
            _animFlag = true;
            _canvasGroup = go.GetComponent<CanvasGroup>();
            _canvasGroup.alpha = _start;
        }

        public override void Update() {
            if (_animFlag) {
                if (Math.Abs(_canvasGroup.alpha - _end) > 0.05f) {
                    _canvasGroup.alpha += ((_end - _start) / _time) * Time.deltaTime;
                } else {
                    _animFlag = false;
                    _canvasGroup.alpha = _end;
                    if(_funcName != "") {
                        _go.SendMessageUpwards(_funcName, new AnimationEventArgs { go = _go, type = _type });
                    }
                    if (_inactiveAfterAnim) {
                        _go.SetActive(false);
                    }

                    SendComplete(this);
                }
            }
        }

        public override string GetStateToString() {
            var timeLeft = ((_end - _canvasGroup.alpha) / _end) * _time;
            return Enum.GetName(typeof(AnimationType), (int)_type) + ";" + _canvasGroup.alpha + ";" + _end + ";" + timeLeft + ";" + _funcName + ";" + Convert.ToString(_inactiveAfterAnim);
        }
    }

    //////////////////////////////////////////
    ///
    ///    ---======= MOVE ======-----
    ///
    //////////////////////////////////////////
    public class MoveAnimation : BaseAnimation {

        private Vector2 _start;
        private Vector2 _target;
        private float _speed;

        private float _fullDist;
        private float _t;

        private bool _animFlag;

        public MoveAnimation(Vector2 start, Vector2 target, float speed, string funcName = "", bool inactiveAfterAnim = false) : base(AnimationType.Move, funcName, inactiveAfterAnim) {
            _start = start;
            _target = target;
            _speed = speed;
        }

        public override void Play(GameObject go) {
            _animFlag = true;
            _go = go;
            _go.transform.position = _start;
            _fullDist = Vector2.Distance(_go.transform.position, _target);
            _t = 0f;
        }

        public override void Update() {
            if (_animFlag) {
                if (_t < 1) {
                    _t += (_fullDist / (_fullDist / _speed)) / _fullDist;
                    _go.transform.position = Vector2.Lerp(_start, _target, _t);
                } else {
                    _animFlag = false;
                    _go.transform.position = _target;
                    if(_funcName != "") {
                        _go.SendMessageUpwards(_funcName, new AnimationEventArgs { go = _go, type = _type });
                    }
                    if (_inactiveAfterAnim) {
                        _go.SetActive(false);
                    }

                    SendComplete(this);
                }
            }
        }

        public override string GetStateToString() {
            return Enum.GetName(typeof(AnimationType), (int)_type) + ";" + _go.transform.position + ";" + _target + ";" + _speed + ";" + _funcName;
        }
    }

    public class AnimationEventArgs {
        public GameObject go;
        public AnimationType type;
    }
}
