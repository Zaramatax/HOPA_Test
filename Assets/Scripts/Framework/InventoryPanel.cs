using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace Framework {
    public class InventoryPanel : MonoBehaviour {

        public static InventoryPanel instance;
    
	    private ScrollRect scrollRect;
        private bool hasChanges = true;
        private List<GameObject> items = new List<GameObject>();
        private InventoryManager inventoryManager;

        public Button leftButton;
        public Button rightButton;
	    public GameObject itemsContainer;

        void Awake()
        {
            instance = this;
            scrollRect = GetComponent<ScrollRect>();
            scrollRect.onValueChanged.AddListener(OnScroll);
        }

        void Start()
        {
            InventoryManager.OnInventoryChanged += instance_OnChange;
		    inventoryManager = InventoryManager.instance;
        }

        public void UpdateState()
        {
            hasChanges = true;
        }

        void InventoryManager_OnSelectionChanged()
        {
        }

        void OnDestroy()
        {
            InventoryManager.OnInventoryChanged -= instance_OnChange;
        }    

        void instance_OnChange()
        {
            hasChanges = true;
        }

        void Update()
        {
            if (hasChanges)
            {
                UpdateInventoryPanel();
                hasChanges = false;
            }
        }

        private void UpdateInventoryPanel()
        {
            ClearPanel();

		    foreach (GameObject item in inventoryManager.CurrentItemsVisible) {
			    GameObject newGo = (GameObject)Instantiate(item);
                Button button = newGo.transform.GetChild(0).gameObject.GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    OnItemSelected(newGo);
                });
			    newGo.transform.SetParent(itemsContainer.transform);
			    items.Add(newGo);
		    }

		    ContentSizeFitter csf = itemsContainer.GetComponent<ContentSizeFitter>();
            bool more = inventoryManager.CurrentItemsVisible.Count > 5;
            csf.horizontalFit = more ? ContentSizeFitter.FitMode.MinSize : ContentSizeFitter.FitMode.Unconstrained;
            if (!more) csf.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 560);
		    if (inventoryManager.CurrentItemsVisible.Count > 6)
                StartCoroutine(scrollToEnd());
        }

        void ClearPanel()
        {
            foreach (Transform item in itemsContainer.transform)
            {
                Destroy(item.gameObject);
            }

            items.Clear();
        }

        void OnItemSelected(GameObject item)
        {
            inventoryManager.SelectItem(item.GetComponent<InventoryItem>());
        }

        IEnumerator scrollToEnd()
        {
            yield return new WaitForEndOfFrame();
            scrollRect.horizontalNormalizedPosition = 1;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            if (!Equals(null))
            {
                gameObject.SetActive(true);
            }
        }

        public void OnScroll(Vector2 vec)
        {
            leftButton.interactable = InventoryManager.instance.CurrentItemsVisible.Count > 6 && vec.x >= 0.05f;
            rightButton.interactable = InventoryManager.instance.CurrentItemsVisible.Count > 6 && vec.x <= 0.95f;
        }

        public void DoScroll(int delta)
        {
            int scrollable = Mathf.Max(InventoryManager.instance.CurrentItemsVisible.Count - 6, 0);
            if (scrollable > 0)
            {
                scrollRect.horizontalNormalizedPosition += delta * 6.0f / scrollable;
                scrollRect.horizontalNormalizedPosition = Mathf.Clamp(scrollRect.horizontalNormalizedPosition, 0, 1);
            }
        }

        public Vector3 GetItemPosition(InventoryItem item) {
            foreach (GameObject place in items) {
                if (place.name == item.itemId + "(Clone)") {
                    return place.transform.position;
                }
            }

            return new Vector3(0.0f, 0.0f, 0.0f);
        }
    }
}