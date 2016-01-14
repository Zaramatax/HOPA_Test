using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Framework {
    public class HOPlaceholder : MonoBehaviour {
        private HOItem item;
        private Image place;
        private bool isFading;
        private const string hideAnimation = "ho_item_on_panel_hide";
        private const string showAnimation = "ho_item_on_panel_show";

        void Start() {
            place = GetComponent<Image>();
        }

        public void Setup(HOItem item) {
            this.item = item;

            GetComponent<Animator>().Play(showAnimation);

            if (item != null)
                place.sprite = item.silhouette;
            else
                gameObject.SetActive(false);

            isFading = false;
        }

        public void UpdateVisibility() {
            gameObject.SetActive(item != null && !item.IsCollected());
        }

        public void FadeOut() {
            gameObject.GetComponent<Animator>().Play(hideAnimation);
            isFading = true;
        }

	    void Update () {
            if (isFading) {
                if (!gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(hideAnimation)) {
                    isFading = false;
                    gameObject.SetActive(false);
                }
            }
	    }

        public void ActivateItem(bool activate) {
            if (item) {
                item.Activate(activate);
            }
        }

        public bool IsItemCollected() {
            bool isc = item != null && item.IsCollected();
            return item != null && item.IsCollected();
        }

        public HOItem GetItem() {
            return item;
        }
    }
}