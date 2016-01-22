using UnityEngine;
using System.Collections;

namespace Framework {
    public class Window : MonoBehaviour {

        public void Open() {
            Animator animator = gameObject.GetComponent<Animator>();
            animator.Play("window_open");
        }

        public void Close() {
            Animator animator = gameObject.GetComponent<Animator>();
            animator.Play("window_close");

            GameObject wm = transform.parent.gameObject;
            WindowManager windowsManager = wm.GetComponent<WindowManager>();
            if (windowsManager) {
                windowsManager.Close();
            }
        }
    }
}