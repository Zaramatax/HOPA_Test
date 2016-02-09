using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework {
    public class CloseSubLocation : MonoBehaviour, IPointerClickHandler {

        public void CloseSub() {
            if (transform.parent.gameObject) {
                transform.parent.gameObject.GetComponent<SubLocation>().Close();
            }
        }

        public void OnPointerClick(PointerEventData eventData) {
            CloseSub();
        }
    }
}