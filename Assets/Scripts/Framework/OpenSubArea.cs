using UnityEngine;
using System.Collections;

namespace Framework {
    public class OpenSubArea : MonoBehaviour {

        public SubLocation subLocation;

        void OnMouseDown() {
            OpenSub();
        }

        void OpenSub() {
            if (subLocation) {
                subLocation.gameObject.GetComponent<SubLocation>().Open();
            }
        }
    }
}