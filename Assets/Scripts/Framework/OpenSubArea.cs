using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework {
    public class OpenSubArea : MonoBehaviour, IPointerClickHandler {

        public SubLocation subLocation;

        public void OnPointerClick (PointerEventData eventData) {
            OpenSub();
        }

        void OpenSub() {
            if (subLocation) {
                subLocation.gameObject.SetActive(true);
                subLocation.gameObject.GetComponent<SubLocation>().Open();
            }
        }
    }
}