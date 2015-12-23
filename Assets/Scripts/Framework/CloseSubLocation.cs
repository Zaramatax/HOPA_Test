using UnityEngine;
using System.Collections;

namespace Framework {
    public class CloseSubLocation : MonoBehaviour {
        void OnMouseDown() {
            CloseSub();
        }

        void CloseSub() {
            if (transform.parent.gameObject) {
                transform.parent.gameObject.GetComponent<SubLocation>().Close();
            }
        }
    }
}