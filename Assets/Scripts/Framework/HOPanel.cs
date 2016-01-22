using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Framework {
    public class HOPanel : MonoBehaviour {

        private List<HOPlaceholder> placeholders;
        private const int visiblePlaceholdersCount = 6;

        public Text description;

        void Awake() {
            placeholders = new List<HOPlaceholder>();
        }

        public void SetupPanel(List<HOItem> items) {
            int i = 0;
            placeholders.Clear();

            foreach (Transform placeholderTransform in transform.GetChild(0)) {
                HOItem item = null;
                if (i < items.Count) {
                    item = items[i];
                }

                HOPlaceholder placeholder = placeholderTransform.gameObject.GetComponent<HOPlaceholder>();
                placeholder.Setup(item);
                placeholders.Add(placeholder);

                ++i;
            }

            OnChange();
        }

        public void OnChange() {
            foreach (HOPlaceholder placeholder in placeholders) {
                placeholder.UpdateVisibility();
            }

            ActivateVisible();
        }

        public void OnCollect(HOItem item) {
            HOPlaceholder placeholder = GetPlaceholder(item);
            if (placeholder != null) {
                placeholder.FadeOut();
            }

            ActivateVisible();
        }

        public Vector3 GetPlaceholderPosition(HOItem item) {
            HOPlaceholder placeholder = GetPlaceholder(item);
            if (placeholder != null) {
                return placeholder.transform.position;
            }

            return new Vector3(0.0f, 0.0f, 0.0f);
        }

        public void ActivateVisible() {
            int i = 0;
            foreach (HOPlaceholder placeholder in placeholders) {
                bool isCollected = placeholder.IsItemCollected();
                placeholder.ActivateItem(i < visiblePlaceholdersCount && !isCollected);

                if (!isCollected) {
                    ++i;
                }
            }

            UpdateText(i);
        }

        HOPlaceholder GetPlaceholder(HOItem item) {
            return placeholders.Find(x => (x.GetItem() == item));
        }

        void UpdateText(int notCollected) {
            description.text = notCollected.ToString() + "/" + placeholders.Count;
        }
    }
}