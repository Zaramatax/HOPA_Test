using UnityEngine;
using System.Collections;

public class ParentTestAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void FadeShowComplete(GameObject go) {
        Debug.Log("FadeShowComplete " + go.name);
    }
}
