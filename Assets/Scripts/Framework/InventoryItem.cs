using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Framework {
    public class InventoryItem : MonoBehaviour {
        public Image bigIcon;
        public string itemId;

        GameObject _icon;
        bool _selected = false;

        void Start() {
            _icon = gameObject.transform.GetChild(0).gameObject;
        }

        public void Select() {
            if (!_selected) {
                _selected = true;
                CursorManager.instance.Attach(_icon);
            }
        }

        public void Unselect() {
            if (_selected) {
                _selected = false;
                CursorManager.instance.Drop();
            }
        }

        public void Drop() {
            if (_selected) {
                _selected = false;
                CursorManager.instance.Detach();
            }
        }

        public bool IsSelected() {
            return _selected;
        }
    }
}