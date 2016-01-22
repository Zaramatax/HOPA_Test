using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Framework {
    public class WellDone : Window {

        public Image itemImage;
        public Text nameText;

        private System.Action action;

        public void Init(InventoryItem item, System.Action action) {
            itemImage.sprite = item.GetSmallImage().sprite;
            nameText.text = item.itemId;

            this.action = action;
        }

        public void OnClick() {
            Close();
            action();
        }
    }
}