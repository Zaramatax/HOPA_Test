using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;

namespace Framework {
    public class InventoryManager : MonoBehaviour {

        public static InventoryManager instance;
        public static event System.Action OnInventoryChanged;

        private Dictionary<GameObject, int> items;
        private InventoryItem selectedItem;
        private InventoryPanel inventoryPanel;

        void Awake() {
            instance = this;
            items = new Dictionary<GameObject, int>();
            selectedItem = null;

            DontDestroyOnLoad(this);
        }

        void Start() {
            inventoryPanel = GameObject.FindObjectOfType<InventoryPanel>();
        }

        void Update() {
            if (Input.GetMouseButtonDown(1) && selectedItem) {
                UnselectItem();
            }
        }

        public void AddItem(string itemId, int count = 1) {
            GameObject itemGO = (GameObject)Resources.Load("Prefabs/Inventory/" + itemId);

            if (itemGO) {
                if (!items.ContainsKey(itemGO)) {
                    items.Add(itemGO, count);
                }
                else {
                    items[itemGO] += count;
                }

                WindowManager.instance.OpenWellDoneWindow(itemGO.GetComponent<InventoryItem>(), Inform);
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
                    if (selectedItem.itemId == itemID) {
                        selectedItem.Drop();
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
            if (selectedItem)
                return selectedItem.itemId;
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
            if (selectedItem) {
                selectedItem.Drop();
            }

            selectedItem = item;
            item.Select();
        }

        public void UnselectItem() {
            selectedItem.Unselect();
            selectedItem = null;
        }

        public Vector3 GetItemPosition(InventoryItem item) {
            return inventoryPanel.GetItemPosition(item);
        }
    }
}