using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework {
    public delegate void MiniHOItemAction(MiniHOItem item);

    public class MiniHOItem : MonoBehaviour {

        public GameObject shadow;
        public GameObject patch;
        public GameObject place;

        public bool collect;

        public event MiniHOItemAction MoveComplete;

        public void Init(RuntimeAnimatorController controller) {
            var animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;

            if (shadow != null) {
                animator = shadow.AddComponent<Animator>();
                animator.runtimeAnimatorController = controller;
            }

            if (place != null) {
                animator = place.AddComponent<Animator>();
                animator.runtimeAnimatorController = controller;
            }

            collect = false;
        }

        private void UpdateState() {
            if (collect) {
                gameObject.SetActive(false);
                if(shadow != null)
                    shadow.SetActive(false);
                if (patch != null)
                    patch.SetActive(false);
                place.SetActive(true);
            } else {
                gameObject.SetActive(true);
                if (shadow != null)
                    shadow.SetActive(true);
                if (patch != null)
                    patch.SetActive(true);
                place.SetActive(false);
            }
        }

        public XmlNode Save(XmlDocument doc) {
            XmlElement collectedInfo = doc.CreateElement("collected");
            collectedInfo.SetAttribute("value", Convert.ToString(collect));
            return collectedInfo;
        }

        public void Load(XmlElement info) {
            collect = Convert.ToBoolean(Utils.GetAttribute(info, "value"));
            UpdateState();
        }

        public void Fly(Vector3 targetPos) {
            collect = true;
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
