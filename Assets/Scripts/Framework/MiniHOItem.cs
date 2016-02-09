using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework {
    public class MiniHOItem : MonoBehaviour {

        private const string fadeShow = "fade_show";
        private const string fadeHide = "fade_hide";

        public GameObject shadow;
        public GameObject patch;

        public event EventHandler ItemOnPlace;

        public void Init(RuntimeAnimatorController controller) {


            var animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;

            if (shadow != null) {
                animator = shadow.AddComponent<Animator>();
                animator.runtimeAnimatorController = controller;
            }

            patch.SetActive(false);
        }

        public void OnClick(Vector3 targetPos) {
            shadow.GetComponent<Animator>().Play(fadeHide);
            StartCoroutine(Fly(targetPos));
        }

        public void OnItemOnPlace() {
            gameObject.SetActive(true);
            GetComponent<Animator>().Play(fadeShow);

            //if (shadow != null) {
            //    var animator = GetComponent<Animator>();
            //    animator.runtimeAnimatorController = controller;
            //}
        }

        IEnumerator Fly(Vector3 targetPos) {
            Vector3 startPosition = transform.position;

            float t = 0;

            while (t <= 1.0f) {
                t += 0.05f;
                transform.position = Vector3.Lerp(startPosition, targetPos, t);

                yield return new WaitForSeconds(0.01f);
            }

            GetComponent<Animator>().Play(fadeHide);

            if (ItemOnPlace != null)
                ItemOnPlace(this, EventArgs.Empty);
        }
    }
}
