using UnityEngine;
using System.Collections;

namespace Framework {
    public class CursorManager : MonoBehaviour {
        public enum CursorMode {
            DEFAULT,
            LOUPE,
            GEARS,
            FINGER,
            DIALOGUE,
            EYE,
        }

        private GameObject draggableObject;
        private GameObject originParent;
        private Vector3 originPosition;
        private Vector3 offset;
        private CursorMode mode;

        public static CursorManager instance;

        void Awake() {
            draggableObject = null;
            mode = CursorMode.DEFAULT;
        }

        void Start() {
            instance = this;
        }

        void Update() {
            if (draggableObject) {
                Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);

                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
                draggableObject.transform.position = curPosition;
            }
        }

        public void Attach(GameObject draggableObject, Vector3 offset = default(Vector3)) {
            this.draggableObject = draggableObject;
            this.originParent = draggableObject.transform.parent.gameObject;
            this.originPosition = draggableObject.transform.position;
            this.offset = offset;

            draggableObject.transform.SetParent(transform);
        }

        public void Drop() {
            StartCoroutine("GetBack");
        }

        public void Detach() {
            draggableObject.transform.position = originPosition;
            draggableObject.transform.SetParent(originParent.transform);

            draggableObject = null;
        }

        IEnumerator GetBack() {
            Vector3 startPosition = draggableObject.transform.position;
            float step = ((startPosition - originPosition).magnitude) * Time.fixedDeltaTime * 0.03f;
            float t = 0;

            while (t <= 1.0f) {
                t += step;
                draggableObject.transform.position = Vector3.Lerp(startPosition, originPosition, t);
                yield return null;
            }

            Detach();
        }

        public void SetMode(CursorMode mode) {
            this.mode = mode;
        }
    }
}