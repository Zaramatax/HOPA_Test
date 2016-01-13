using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Framework {
    public class HOPanel : MonoBehaviour {

        private List<HOPlaceholder> placeholders;

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
        }

        public void OnChange() {
            foreach (HOPlaceholder placeholder in placeholders) {
                placeholder.UpdateVisibility();
            }
        }

        public void OnCollect(HOItem item) {
            HOPlaceholder placeholder = placeholders.Find(x => (x.item == item));
            if (placeholder != null) {
                placeholder.FadeOut();
            }
        }

        public Vector3 GetPlaceholderPosition(HOItem item) {
            HOPlaceholder placeholder = placeholders.Find(x => (x.item == item));
            if (placeholder != null) {
                return placeholder.transform.position;
            }

            return new Vector3(0.0f, 0.0f, 0.0f);
        }
    }
}