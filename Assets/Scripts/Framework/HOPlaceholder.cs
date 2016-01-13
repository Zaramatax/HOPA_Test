using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Framework {
    public class HOPlaceholder : MonoBehaviour {
        public HOItem item;

        private Image place;
        private bool isFading;
        private const string hideAnimation = "ho_item_on_panel_hide";
        void Start() {
            place = GetComponent<Image>();
        }

        public void Setup(HOItem item) {
            this.item = item;

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
    }
}