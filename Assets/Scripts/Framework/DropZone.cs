using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Framework {
    public class DropZone : MonoBehaviour, IPointerClickHandler {

        public InventoryItem requiredItem;
        public int itemsCount = 1;

        [LocalizationString]
        public string clickMessage;

        public UnityEvent onUse;
        private InventoryManager inventory;

        void Start() {
            inventory = InventoryManager.instance;
        }

        public void OnPointerClick (PointerEventData eventData) {
            if (inventory.GetSelectedItem() == requiredItem.itemId && itemsCount == inventory.GetItemsCount(requiredItem.itemId)) {
                onUse.Invoke();
                inventory.RemoveItem(requiredItem.itemId);

                return;
            }

            CommentManager.instance.Show(clickMessage);
        }
    }
}