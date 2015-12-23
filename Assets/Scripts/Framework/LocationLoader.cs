using UnityEngine;
using System.Collections;

namespace Framework {
    public class LocationLoader : MonoBehaviour {
        void Start() {
            StartCoroutine("Load");
        }

        IEnumerator Load() {
            AsyncOperation async = Application.LoadLevelAsync(LocationManager.instance.GetLocationToLoad());
            yield return null;
            LocationManager.instance.LocationLoaded();
        }
    }
}