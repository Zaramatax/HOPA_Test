using UnityEngine;
using System.Collections;

namespace Framework {
    public class CursorManager : MonoBehaviour {

        GameObject _draggable_object;
        GameObject _origin_parent;
        Vector3 _origin_position;
        Vector3 _offset;

        public static CursorManager instance;

        void Awake() {
            _draggable_object = null;
        }

        void Start() {
            instance = this;
        }

        void Update() {
            if (_draggable_object) {
                Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);

                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + _offset;
                _draggable_object.transform.position = curPosition;
            }
        }

        public void Attach(GameObject draggable_object, Vector3 offset = default(Vector3)) {
            _draggable_object = draggable_object;
            _origin_parent = draggable_object.transform.parent.gameObject;
            _origin_position = draggable_object.transform.position;
            _offset = offset;

            _draggable_object.transform.SetParent(transform);
        }

        public void Drop() {
            StartCoroutine("GetBack");
        }

        public void Detach() {
            _draggable_object.transform.position = _origin_position;
            _draggable_object.transform.SetParent(_origin_parent.transform);

            _draggable_object = null;
        }

        IEnumerator GetBack() {
            Vector3 startPosition = _draggable_object.transform.position;
            float step = ((startPosition - _origin_position).magnitude) * Time.fixedDeltaTime * 0.03f;
            float t = 0;

            while (t <= 1.0f) {
                t += step;
                _draggable_object.transform.position = Vector3.Lerp(startPosition, _origin_position, t);
                yield return null;
            }

            Detach();
        }
    }
}