using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Framework {
    public class DropZone : MonoBehaviour {

        public InventoryItem requiredItem;
        public int itemsCount = 1;
        public string clickMessage;
        public UnityEvent onUse;

        private InventoryManager inventory;

        void Start() {
            inventory = InventoryManager.instance;
        }

        void OnMouseDown() {
            if (inventory.GetSelectedItem() == requiredItem.itemId && itemsCount == inventory.GetItemsCount(requiredItem.itemId)) {
                onUse.Invoke();
                inventory.RemoveItem(requiredItem.itemId);

                return;
            }

            //TO DO: show note
        }
    }
}