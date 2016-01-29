using UnityEngine;
using System;
using UnityEngine.UI;

namespace Framework {
    public class BlackStripes : MonoBehaviour {
        private static string showBlackStripes = "show_black_stripes";
        private static string hideBlackStripes = "hide_black_stripes";

        public Action onShow;
        public Action onHide;
        
        public void Show() {
            gameObject.GetComponent<Animator>().Play(showBlackStripes);
        }

        public void Hide () {
            gameObject.GetComponent<Animator>().Play(hideBlackStripes);
        }

        public void OnShow () {
            if (onShow != null) {
                onShow();
            }
        }

        public void OnHide () {
            if (onHide != null) {
                onHide();
            }
        }

        void OnMouseDown() {
            int i = 0; ++i;
        }
    }
}