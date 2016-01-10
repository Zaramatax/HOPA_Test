using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;

namespace Framework {
    enum HOMode {
        DEFAULT,
        SILHOUETTE,
        CLOSED_BOXES,
    }

    public class HOManager : MonoBehaviour {

        GameObject hoPanelGO;
        GameObject inventoryPanelGO;
        HOPanel hoPanel;
        HOMode mode;
        public List<HOItem> items;

        void Start() {
            hoPanelGO = GameObject.Find("HOPanel");
            inventoryPanelGO = GameObject.Find("BottomPanel");

            Assert.IsNotNull(hoPanelGO, "Fatal error: HO panel not found");
            Assert.IsNotNull(inventoryPanelGO, "Fatal error: inventory panel not found");

            hoPanel = hoPanelGO.GetComponent<HOPanel>();

            Assert.IsNotNull(hoPanel, "Fatal error: no 'HOPanel' component found");

            hoPanel.SetUpPanel(items);

            ChangePanels(true);

            foreach (HOItem item in items) {
                item.onCollect += OnCollect;
                item.onCollectAnimationEnded += OnCollectAnimationEnded;
            }
        }

        void ChangePanels(bool toHO) {
            string playHO = "open", playInventory = "close";
            if (!toHO) {
                playInventory = "open";
                playHO = "close";
            }           

            if (hoPanelGO) {
                hoPanelGO.GetComponent<Animator>().Play(playHO);
            }

            if (inventoryPanelGO) {
                inventoryPanelGO.GetComponent<Animator>().Play(playInventory);
            }
        }

        void OnDestroy() {
            foreach (HOItem item in items) {
                item.onCollect -= OnCollect;
                item.onCollectAnimationEnded -= OnCollectAnimationEnded;
            }

            ChangePanels(false);
        }

        void OnCollect(HOItem item) {
            item.FLyToPanel(hoPanel.GetPlaceholderPosition(item));
        }

        void OnCollectAnimationEnded(HOItem item) {
            hoPanel.OnCollect(item);
        } 
    }
}