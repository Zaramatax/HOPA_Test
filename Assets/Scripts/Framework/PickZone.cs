using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework {
    public class PickZone : MonoBehaviour, IPointerClickHandler {

        public InventoryItem item;
        public int count = 1;

        public void OnPointerClick(PointerEventData eventData) {
            InventoryManager.instance.AddItem(item.itemId, count);
            gameObject.SetActive(false);
        }
    }
}