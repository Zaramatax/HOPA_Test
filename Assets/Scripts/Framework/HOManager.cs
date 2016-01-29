using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System.Xml;

namespace Framework {
    [System.Serializable]
    public class HOList {
        public List<HOItem> Items;
    }

    public class HOManager : Location {

        private GameObject hoPanelGO;
        private GameObject inventoryPanelGO;
        private HOPanel hoPanel;
        private const string stateNode = "ho_state";
        private const string currentListAttribute = "current_list";
        private const string openAnimation = "open";
        private const string closeAnimation = "close";
        private int currentList;

        public List<HOList> lists;
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

            hoPanel.SetupPanel(lists[currentList].Items);

            ChangePanels(true);

            foreach (HOList list in lists) {
                foreach (HOItem item in list.Items) {
                    item.onCollect += OnCollect;
                    item.onCollectAnimationEnded += OnCollectAnimationEnded;
                }
            }
        }

        override protected void OnDestroy() {
            base.OnDestroy();

            foreach (HOList list in lists) {
                foreach (HOItem item in list.Items) {
                    item.onCollect -= OnCollect;
                    item.onCollectAnimationEnded -= OnCollectAnimationEnded;
                }
            }

            ChangePanels(false);
        }

        override protected void Save() {
            base.Save();

            XmlDocument doc = new XmlDocument();
            
            if (ProfileSaver.Load(doc, locationName)) {
                SaveHOState(doc);
            }
            ProfileSaver.Save(doc, locationName);
        }

        void SaveHOState(XmlDocument doc) {
            XmlNode hoState = doc.CreateElement(stateNode);
            XmlAttribute attribute = doc.CreateAttribute(currentListAttribute);
            attribute.Value = currentList.ToString();
            hoState.Attributes.Append(attribute);

            foreach (HOItem item in lists[currentList].Items) {
                item.SaveToXML(doc, hoState);
            }

            doc.DocumentElement.AppendChild(hoState);
        }

        override protected void Load() {
            base.Load();

            XmlDocument doc = new XmlDocument();
            
            if (ProfileSaver.Load(doc, locationName)) {
                LoadHOState(doc);
            }
        }

        void LoadHOState(XmlDocument doc) {
            XmlNode hoStateNode = doc.DocumentElement.SelectSingleNode(stateNode);
            currentList = int.Parse(hoStateNode.Attributes[currentListAttribute].Value);

            foreach (HOItem item in lists[currentList].Items) {
                item.LoadFromXML(doc, hoStateNode);
            }
        }

        void ChangePanels(bool toHO) {
            string playHO = openAnimation, playInventory = closeAnimation;
            if (!toHO) {
                playInventory = openAnimation;
                playHO = closeAnimation;
            }

            if (hoPanelGO) {
                hoPanelGO.GetComponent<Animator>().Play(playHO);
            }

            if (inventoryPanelGO) {
                inventoryPanelGO.GetComponent<Animator>().Play(playInventory);
            }
        }

        void OnCollect(HOItem item) {
            item.FLyToPanel(hoPanel.GetPlaceholderPosition(item));
        }

        void OnCollectAnimationEnded(HOItem item) {
            hoPanel.OnCollect(item);
            CheckComplete();
        }

        void CheckComplete() {
            foreach (HOItem item in lists[currentList].Items) {
                if (!item.IsCollected()) {
                    return;
                }
            }
            
            if (currentList == lists.Count - 1) {
                Complete();
            }
            else {
                ++currentList;
                hoPanel.SetupPanel(lists[currentList].Items);
            }
        }

        void Complete() {
            LocationManager.instance.GoToLocation(parentLocation);
        }
    }
}