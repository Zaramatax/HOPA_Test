using UnityEngine;
using System.Collections;
using AssetBundles;
using UnityEngine.SceneManagement;
using UnityEditor;

using Framework;

public class LoadScenes : MonoBehaviour
{
    void Start() {
        string sceneName = LocationManager.instance.GetLocationToLoad();

        if (SceneExists(sceneName)) {
            StartCoroutine(LoadLocal(sceneName));
        }
        else {
            StartCoroutine(LoadBundle(sceneName));
        }
    }

    bool SceneExists(string sceneName) {
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        foreach (EditorBuildSettingsScene scene in scenes) {
            if (scene.path == "Assets/Scenes/" + sceneName + ".unity")
                return true;
        }

        return false;
    }

    IEnumerator LoadLocal (string sceneName) {
        SceneManager.LoadSceneAsync(sceneName);
        yield return null;
        LocationManager.instance.LocationLoaded();
    }

    // Use this for initialization
    IEnumerator LoadBundle (string sceneName) {
        yield return StartCoroutine(Initialize());

            // Load level.
        yield return StartCoroutine(InitializeLevelAsync(sceneName, false));
	}

	// Initialize the downloading url and AssetBundleManifest object.
	protected IEnumerator Initialize()
	{
		// Don't destroy this gameObject as we depend on it to run the loading script.
		//DontDestroyOnLoad(gameObject);
		
		// With this code, when in-editor or using a development builds: Always use the AssetBundle Server
		// (This is very dependent on the production workflow of the project. 
		// 	Another approach would be to make this configurable in the standalone player.)
		#if DEVELOPMENT_BUILD || UNITY_EDITOR
		AssetBundleManager.SetDevelopmentAssetBundleServer ();
		#else
		// Use the following code if AssetBundles are embedded in the project for example via StreamingAssets folder etc:
		AssetBundleManager.SetSourceAssetBundleURL(Application.dataPath + "/");
		// Or customize the URL based on your deployment or configuration
		//AssetBundleManager.SetSourceAssetBundleURL("http://www.MyWebsite/MyAssetBundles");
		#endif
		
		// Initialize AssetBundleManifest which loads the AssetBundleManifest object.
		var request = AssetBundleManager.Initialize();
		
		if (request != null)
			yield return StartCoroutine(request);
	}

	protected IEnumerator InitializeLevelAsync (string sceneName, bool isAdditive)
	{
		// This is simply to get the elapsed time for this phase of AssetLoading.
		float startTime = Time.realtimeSinceStartup;

		// Load level from assetBundle.
		AssetBundleLoadOperation request = AssetBundleManager.LoadLevelAsync(sceneName, sceneName, isAdditive);
		if (request == null)
			yield break;
		StartCoroutine(request);
        yield return null;

        // Calculate and display the elapsed time.
        float elapsedTime = Time.realtimeSinceStartup - startTime;
		Debug.Log("Finished loading scene " + sceneName + " in " + elapsedTime + " seconds" );

        LocationManager.instance.LocationLoaded();
    }
}
