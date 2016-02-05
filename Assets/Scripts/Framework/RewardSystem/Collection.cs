using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Framework {
    public class Collection : MonoBehaviour {
        public const string textPath = "gameplay/collections";

        [SerializeField]
        private Sprite icon;
        [SerializeField]
        private List<CollectionItem> items;

        public string id { get; private set; }

        public int ItemsCount { get { return items.Count; } }
        public Sprite Icon { get { return icon; } }
        public string Title { get { return "{" + textPath + "/" + id + "/title}"; } }

        public void Init() {
            id = name;
            items.ForEach(x => x.Init());
        }

        public CollectionItem GetItem(int index) {
            try {
                return items[index];
            } catch {
                throw new Exception("Collection item Index is out of range: index = '" + index + "', collection = '" + id + "'.");
            }
        }

        public XmlNode Save(XmlDocument doc) {
            XmlElement collection = doc.CreateElement(id);
            items.ForEach(x => collection.AppendChild(x.Save(doc)));
            return collection;
        }

        public void Load(XmlElement collection) {
            if (collection == null) return;

            for(int i = 0; i < items.Count; i++) {
                items[i].Load((XmlElement)collection.ChildNodes[i]);
            }

            //foreach(CollectionItem item in items) {
            //    collection.
            //}

            //foreach (XmlElement item in collection) {
            //    var currentItem = items.Find(x => x.id == item.Name);
            //    if (currentItem == null) continue;

            //    currentItem.Load(item);
            //}
        }
    }


    [Serializable]
    public class CollectionItem : IReward {
        [SerializeField]
        private Sprite icon;
        [SerializeField]
        private int score;

        public Sprite Icon { get { return icon; } }

        public bool collected { get; private set; }

        public void Init() {
            collected = false;
        }

        public void MarkAsCollected() {
            collected = true;
            TryGiveReward();
        }

        public void TryGiveReward() {
            RewardManager.Instance.GiveReward(this);
        }

        public int GetScore() {
            return score;
        }

        public XmlNode Save(XmlDocument doc) {
            XmlElement item = doc.CreateElement("collection_item");
            item.SetAttribute("collected", Convert.ToString(collected));
            return item;
        }

        public void Load(XmlElement item) {
            if (item == null) return;

            var collectedValue = item.GetAttribute("collected");
            if (collectedValue != null)
                collected = Convert.ToBoolean(collectedValue);
        }
    }
}