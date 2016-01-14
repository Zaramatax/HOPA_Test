using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Xml;

namespace Framework {
    enum State {
        INACTIVE,
        ACTIVE,
        COLLECTED,
        FLYING,
        ENDED,
    }
    public class HOItem : MonoBehaviour, IPointerClickHandler {
        private State state;
        private Vector3 endPosition;
        private const string stateAttribute = "state";

        public Sprite silhouette;
        public event System.Action<HOItem> onCollect;
        public event System.Action<HOItem> onCollectAnimationEnded;

        void Awake() {
            state = State.INACTIVE;
            endPosition = transform.position;
        }

        public void Activate(bool active) {
            if (State.INACTIVE == state && active) {
                state = State.ACTIVE;
            }
        }

        void Collect() {
            state = State.COLLECTED;

            if (onCollect != null) {
                onCollect(this);
            }
        }

        public bool IsCollected() {
            return State.ENDED == state;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (State.ACTIVE == state) {
                Collect();
            }
        }

        public void FLyToPanel(Vector3 position) {
            state = State.FLYING;
            endPosition = position;
            StartCoroutine(Fly());
        }
        
        IEnumerator Fly() {
            Vector3 startPosition = transform.position;

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

        public void SaveToXML(XmlDocument doc, XmlNode hoState) {
            XmlNode node = doc.CreateElement(gameObject.name.Replace(' ', '_'));
            XmlAttribute attribute = doc.CreateAttribute(stateAttribute);
            attribute.Value = state.ToString();
            node.Attributes.Append(attribute);

            hoState.AppendChild(node);
        }

        public void LoadFromXML(XmlDocument doc, XmlNode hoStateNode) {
            XmlNode node = hoStateNode.SelectSingleNode(gameObject.name.Replace(' ', '_'));
            if (node != null) {
                XmlAttribute attribute = node.Attributes[stateAttribute];
                if (attribute == null) {
                    return;
                }

                state = Utils.ParseEnum<State>(attribute.Value);

                if (State.COLLECTED == state || State.FLYING == state) {
                    state = State.ENDED;
                }

                if (State.ENDED == state) {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}