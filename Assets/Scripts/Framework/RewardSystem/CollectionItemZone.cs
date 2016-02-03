using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework {
    public class CollectionItemZone : MonoBehaviour, IPointerClickHandler {
        public string collectionId;
        public string itemId;

        void Start() {
            if(RewardManager.Instance.IsCollected(collectionId, itemId))
                gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData) {
            RewardManager.Instance.CollectItem(collectionId, itemId);
            gameObject.SetActive(false);
        }
    }
}
