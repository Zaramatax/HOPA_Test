using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework {
    public class GoToLocation : MonoBehaviour, IPointerClickHandler {

        public string locationName;

        public void OnPointerClick (PointerEventData eventData) {
            LocationManager.instance.GoToLocation(locationName);
        }
    }
}