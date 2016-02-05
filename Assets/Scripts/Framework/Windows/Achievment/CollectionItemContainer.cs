using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Framework {
    class CollectionItemContainer : MonoBehaviour {

        public Image icon;

        private CollectionItem item;

        public void Setup(CollectionItem item) {
            this.item = item;
            icon.sprite = item.Icon;
        }

        public void RefreshDisplay() {
            if (!item.collected) {
                icon.color = new Color(1, 1, 1, 0.5f);
            }
        }

    }
}
