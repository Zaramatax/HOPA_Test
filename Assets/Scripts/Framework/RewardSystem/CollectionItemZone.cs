using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework {
    public class CollectionItemZone : MonoBehaviour, IPointerClickHandler {
        public string collectionId;
        public int itemIndex;

        void Start() {
            var item = RewardManager.Instance.GetCollection(collectionId).GetItem(itemIndex);

            if (item.collected)
                gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData) {
            RewardManager.Instance.CollectItem(collectionId, itemIndex);
            gameObject.SetActive(false);
        }
    }
}
