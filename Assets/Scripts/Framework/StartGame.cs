using UnityEngine;
using System.IO;

namespace Framework {
    public class StartGame : MonoBehaviour {

        public string startLocation;
        public bool deleteProfileOnLaunch;

        void Awake() {
            DontDestroyOnLoad(gameObject);
            LocationState.CreateSavesList();
        }

        void Start() {
            if(deleteProfileOnLaunch) {
                Directory.Delete(ProfileSaver.ProfilePath, true);
            }

            LocationManager.instance.GoToLocation(startLocation);
        }
    }
}