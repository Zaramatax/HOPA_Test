using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Framework {
    enum State {
        ACTIVE,
        COLLECTED,
        FLYING,
        ENDED,
    }
    public class HOItem : MonoBehaviour, IPointerClickHandler {
        private State state;
        private Vector3 endPosition;

        public Sprite silhouette;
        public event System.Action<HOItem> onCollect;
        public event System.Action<HOItem> onCollectAnimationEnded;

        void Awake() {
            state = State.ACTIVE;
            endPosition = transform.position;
        }

        void Collect() {
            state = State.COLLECTED;

            if (onCollect != null) {
                onCollect(this);
            }
        }

        public bool IsCollected() {
            return State.COLLECTED == state;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Collect();
        }

        public void FLyToPanel(Vector3 position) {
            state = State.FLYING;
            endPosition = position;
            StartCoroutine(Fly());
        }
        
        IEnumerator Fly() {
            Vector3 startPosition = transform.position;

            float step = ((startPosition - endPosition).magnitude) * Time.fixedDeltaTime * 0.01f;
            float t = 0;

            while (t <= 1.0f) {
                t += 0.05f;
                transform.position = Vector3.Lerp(startPosition, endPosition, t);

                yield return new WaitForSeconds(0.01f);
            }

            state = State.ENDED;

            if (onCollectAnimationEnded != null) {
                onCollectAnimationEnded(this);
            }

            gameObject.SetActive(false);
        }
    }
}