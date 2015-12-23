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

        InventoryManager _inventory;

        void Start() {
            _inventory = InventoryManager.instance;
        }

        void OnMouseDown() {
            if (_inventory.GetSelectedItem() == requiredItem.itemId && itemsCount == _inventory.GetItemsCount(requiredItem.itemId)) {
                onUse.Invoke();
                _inventory.RemoveItem(requiredItem.itemId);

                return;
            }

            //TO DO: show note
        }
    }
}