using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Framework {
    enum HOMode {
        DEFAULT,
        SILHOUETTE,
        CLOSED_BOXES,
    }

    public class HOManager : Location {

        GameObject hoPanelGO;
        GameObject inventoryPanelGO;
        HOPanel hoPanel;
        HOMode mode;

        public List<HOItem> items;
        public string parentLocation;

        override protected void Start() {
            base.Start();

            hoPanelGO = GameObject.Find("HOPanel");
            inventoryPanelGO = GameObject.Find("BottomPanel");

            Assert.IsNotNull(hoPanelGO, "Fatal error: HO panel not found");
            Assert.IsNotNull(inventoryPanelGO, "Fatal error: inventory panel not found");
            Assert.IsNotNull(parentLocation, "Fatal error: HO scene must have parent");

            hoPanel = hoPanelGO.GetComponent<HOPanel>();

            Assert.IsNotNull(hoPanel, "Fatal error: no 'HOPanel' component found");

            hoPanel.SetUpPanel(items);
            hoPanel.OnChange();

            ChangePanels(true);

            foreach (HOItem item in items) {
                item.onCollect += OnCollect;
                item.onCollectAnimationEnded += OnCollectAnimationEnded;
            }
        }

        override protected void Save() {
            base.Save();

            XmlDocument doc = new XmlDocument();

            try {
                doc.Load(locationName + ".xml");
                SaveHOState(doc);
                doc.Save(locationName + ".xml");
            }
            catch { };
        }

        void SaveHOState(XmlDocument doc) {
            XmlNode hoState = doc.CreateElement("ho_state");

            foreach (HOItem item in items) {
                item.SaveToXML(doc, hoState);                
            }

            doc.DocumentElement.AppendChild(hoState);
        }

        override protected void Load() {
            base.Load();

            XmlDocument doc = new XmlDocument();

            try {
                doc.Load(locationName + ".xml");
                LoadHOState(doc);
            }
            catch { };
        }

        void LoadHOState(XmlDocument doc) {
            XmlNode hoStateNode = doc.DocumentElement.SelectSingleNode("ho_state");

            foreach (HOItem item in items) {
                item.LoadFromXML(doc, hoStateNode);
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

        override protected void OnDestroy() {
            base.OnDestroy();

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
            CheckComplete();
        }

        void CheckComplete() {
            foreach (HOItem item in items) {
                if (!item.IsCollected()) {
                    return;
                }
            }

            Complete();
        }

        void Complete() {
            LocationManager.instance.GoToLocation(parentLocation);
        }
    }
}