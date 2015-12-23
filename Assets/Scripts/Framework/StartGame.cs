using UnityEngine;
using System.Collections;

namespace Framework {
    public class StartGame : MonoBehaviour {

        public string startLocation;

        void Awake() {
            DontDestroyOnLoad(gameObject);
            LocationState.CreateSavesList();
        }
        void Start() {
            LocationManager.instance.GoToLocation(startLocation);
        }
    }
}