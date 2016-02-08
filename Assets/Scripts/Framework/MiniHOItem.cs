using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework {
    public class MiniHOItem : MonoBehaviour {

        public GameObject shadow;
        public GameObject patch;

        public event EventHandler ItemOnPlace;

        public void OnClick(Vector3 targetPos) {
            StartCoroutine(Fly(targetPos));
        }

        public void OnItemOnPlace() {
            gameObject.SetActive(true);
        }

        IEnumerator Fly(Vector3 targetPos) {
            Vector3 startPosition = transform.position;

            float t = 0;

            while (t <= 1.0f) {
                t += 0.05f;
                transform.position = Vector3.Lerp(startPosition, targetPos, t);

                yield return new WaitForSeconds(0.01f);
            }

            if (ItemOnPlace != null)
                ItemOnPlace(this, EventArgs.Empty);
        }
    }
}
