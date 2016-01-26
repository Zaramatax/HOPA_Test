using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Framework {
    public class CommentManager : MonoBehaviour {
        private enum State {OPEN, CLOSED, };

        private const string show = "show_comment";
        private const string hide = "hide_comment";

        public Animator underlayer;
        public Text text;
        public static CommentManager instance;

        private State state;

        void Start () {
            if (this != instance) {
                instance = this;
            }

            state = State.CLOSED;
        }

        public void Show (string message) {
            text.text = LocalizationManager.GetTranslationStatic(message);
            StartCoroutine(Show());
        }

        public void Show(List<string> messages) {
            int randomMessage = Random.Range(0, messages.Count);
            Show(messages[randomMessage]);
        }

        IEnumerator Show () {
            if (State.CLOSED == state) {
                underlayer.Play(show);
            }
            state = State.OPEN;

            yield return new WaitForSeconds(3);

            underlayer.Play(hide);
            state = State.CLOSED;
        }
    }
}