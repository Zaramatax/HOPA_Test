using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;

namespace Framework {
    public class InventoryManager : MonoBehaviour {

        public static InventoryManager instance;
        public static event System.Action OnInventoryChanged;

        Dictionary<GameObject, int> items = new Dictionary<GameObject, int>();
        InventoryItem _selectedItem = null;

        void Awake() {
            instance = this;
            items.Clear();
            DontDestroyOnLoad(this);
        }

        void Update() {
            if (Input.GetMouseButtonDown(1) && _selectedItem) {
                UnselectItem();
            }
        }

        public void AddItem(string itemId) {
            GameObject itemGO = (GameObject)Resources.Load("Prefabs/Inventory/" + itemId);

            if (itemGO) {
                if (!items.ContainsKey(itemGO)) {
                    items.Add(itemGO, 1);
                }
                else {
                    items[itemGO]++;
                }

                Inform();
            }
            else {
                Debug.LogWarning("Not found item " + itemId);
            }
        }

        public void RemoveItem(string itemID, bool use = true) {
            List<GameObject> itemsKeys = new List<GameObject>(items.Keys);
            GameObject item = itemsKeys.Find(x => x.GetComponent<InventoryItem>().itemId == itemID);

            if (item) {
                if (use) {
                    items[item] = 0;
                }
                else {
                    items[item] -= 1;
                }

                if (items[item] == 0) {
                    if (_selectedItem.itemId == itemID) {
                        _selectedItem.Drop();
                    }
                    items.Remove(item);
                    Inform();
                }
            }
        }

        public void RemoveAll() {
            items.Clear();
            Inform();
        }

        public InventoryItem GetItem(string itemId) {
            foreach (KeyValuePair<GameObject, int> item in items) {
                InventoryItem ii = item.Key.GetComponent<InventoryItem>();
                if (itemId == ii.itemId)
                    return ii;
            }

            return null;
        }

        public List<string> CurrentItemIds {
            get {
                var res = new List<string>();

                foreach (KeyValuePair<GameObject, int> item in items)
                    if (item.Value > 0)
                        res.Add(item.Key.GetComponent<InventoryItem>().itemId);

                return res;
            }
        }

        public List<GameObject> CurrentItemsVisible {
            get {
                var res = new List<GameObject>();

                foreach (KeyValuePair<GameObject, int> item in items)
                    if (item.Value > 0) {
                        res.Add(item.Key);
                    }

                return res;
            }
        }

        public bool IsHidden(string itemId) {
            return false;
        }

        public void SetHidden(string item, bool hidden) {
        }

        public void Inform() {
            if (OnInventoryChanged != null)
                OnInventoryChanged();
        }

        public string GetSelectedItem() {
            if (_selectedItem)
                return _selectedItem.itemId;
            else
                return "";
        }

        public int GetItemsCount(string itemId) {
            foreach (KeyValuePair<GameObject, int> item in items)
                if (item.Key.GetComponent<InventoryItem>().itemId == itemId)
                    return item.Value;

            return 0;
        }

        public void SelectItem(InventoryItem item) {
            if (_selectedItem) {
                _selectedItem.Drop();
            }

            _selectedItem = item;
            item.Select();
        }

        public void UnselectItem() {
            _selectedItem.Unselect();
            _selectedItem = null;
        }
    }
}