using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework {
    public class CommentZone : MonoBehaviour, IPointerClickHandler {

        [LocalizationString]
        public string comment;

        private CommentManager commentManager;

        void Start() {
            commentManager = CommentManager.instance;
        }

        public void OnPointerClick (PointerEventData eventData) {
            commentManager.Show(comment);
        }
    }
}