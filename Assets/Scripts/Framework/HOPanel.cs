using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Framework {

    public struct Placeholder {
        public Image place;
        public HOItem item;

        public Placeholder(Image place, HOItem item) {
            this.place = place;
            this.item = item;

            this.place.sprite = this.item.silhouette;
        }

        public void UpdateColor() {
            if (item.IsCollected()) {
                place.gameObject.GetComponent<Animator>().Play("ho_item_on_panel_hide");
            }
            else {
                place.gameObject.GetComponent<Animator>().Play("ho_item_on_panel_show");
            }
        }
    }

    public class HOPanel : MonoBehaviour {

        public List<Placeholder> placeholders;

        void Awake() {
            placeholders = new List<Placeholder>();
        }

        public void SetUpPanel(List<HOItem> items) {
            int i = 0;
            placeholders.Clear();

            foreach (Transform placeholder in transform.GetChild(0)) {
                if (i >= items.Count) {
                    break;
                }

                Image place = placeholder.gameObject.GetComponent<Image>();
                placeholders.Add(new Placeholder(place, items[i]));

                ++i;
            }
        }

        public void OnChange() {
            foreach (Placeholder placeholder in placeholders) {
                placeholder.UpdateColor();
            }
        }

        public void OnCollect(HOItem item) {
            foreach (Placeholder placeholder in placeholders) {
                if (item == placeholder.item) {
                    placeholder.place.gameObject.GetComponent<Animator>().Play("ho_item_on_panel_hide");
                } 
            }
        }

        public Vector3 GetPlaceholderPosition(HOItem item) {
            foreach (Placeholder placeholder in placeholders) {
                if (item == placeholder.item) {
                    return placeholder.place.transform.position;
                }
            }

            return new Vector3(0.0f, 0.0f, 0.0f);
        } 
    }
}