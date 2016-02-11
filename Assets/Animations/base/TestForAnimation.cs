using UnityEngine;
using System.Collections;

namespace Framework {
    public class TestForAnimation : MonoBehaviour {

        // Use this for initialization
        void Start() {
            
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetMouseButtonUp(0)) {
                AnimationSystem.Instance.Move(gameObject, new Vector2(-300,-300), new Vector2(300,300), 20f);
            }
        }

        public void FadeShowComplete() {
            Debug.Log("FadeShowComplete " + name);
        }
    }
}
