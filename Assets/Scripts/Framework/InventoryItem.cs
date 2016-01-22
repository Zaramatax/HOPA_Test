using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Framework {
    public class InventoryItem : MonoBehaviour {
        public Image bigIcon;
        public string itemId;

        private GameObject icon;
        private bool selected = false;

        void Start() {
            icon = gameObject.transform.GetChild(0).gameObject;
        }

        public void Select() {
            if (!selected) {
                selected = true;
                CursorManager.instance.Attach(icon);
            }
        }

        public void Unselect() {
            if (selected) {
                selected = false;
                CursorManager.instance.Drop();
            }
        }

        public void Drop() {
            if (selected) {
                selected = false;
                CursorManager.instance.Detach();
            }
        }

        public bool IsSelected() {
            return selected;
        }

        public Image GetSmallImage() {
            Transform icon = transform.GetChild(0);
            return icon.gameObject.GetComponent<Image>();
        }
    }
}