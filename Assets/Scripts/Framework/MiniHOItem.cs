using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework {
    public delegate void MiniHOItemAction(MiniHOItem item);

    public class MiniHOItem : MonoBehaviour {

        public GameObject shadow;
        public GameObject patch;

        public event MiniHOItemAction MoveComplete;

        public void Init(RuntimeAnimatorController controller) {
            var animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;

            if (shadow != null) {
                animator = shadow.AddComponent<Animator>();
                animator.runtimeAnimatorController = controller;
            }
        }

        public void OnFadeHideComplete(GameObject go) {
            go.SetActive(false);
        }

        public void Fly(Vector3 targetPos) {
            StartCoroutine(MoveItem(targetPos));
        }

        IEnumerator MoveItem(Vector3 targetPos) {
            Vector3 startPosition = transform.position;

            float t = 0;

            while (t <= 1.0f) {
                t += 0.05f;
                transform.position = Vector3.Lerp(startPosition, targetPos, t);

                yield return new WaitForSeconds(0.01f);
            }

            if (MoveComplete != null)
                MoveComplete(this);
        }
    }
}
