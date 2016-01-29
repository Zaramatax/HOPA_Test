using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Framework {
    public class LocationLoader : MonoBehaviour {
        void Start() {
            StartCoroutine(Load());
        }

        IEnumerator Load() {
            SceneManager.LoadSceneAsync(LocationManager.instance.GetLocationToLoad());
            yield return null;
            LocationManager.instance.LocationLoaded();
        }
    }
}