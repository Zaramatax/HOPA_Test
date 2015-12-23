using UnityEngine;
using System.Collections;

namespace Framework {
    public class GoToLocation : MonoBehaviour {

        public string locationName;

        void OnMouseDown() {
            LocationManager.instance.GoToLocation(locationName);
        }
    }
}