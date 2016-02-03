using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Framework {
    public class Collection : MonoBehaviour {
        public string id;
        public Sprite icon;
        public List<CollectionItem> items;

        public void Init() {
            items.ForEach(x => x.Init());
        }

        public XmlNode Save(XmlDocument doc) {
            XmlElement collection = doc.CreateElement(id);
            items.ForEach(x => collection.AppendChild(x.Save(doc)));
            return collection;
        }

        public void Load(XmlElement collectionInfo) {
            if (collectionInfo == null) return;

            foreach (XmlElement itemInfo in collectionInfo) {
                var currentItem = items.Find(x => x.id == itemInfo.Name);
                if (currentItem == null) continue;

                currentItem.Load(itemInfo);
            }
        }
    }

    [Serializable]
    public class CollectionItem {
        public string id;
        public Sprite icon;
        public int score;

        [HideInInspector]
        public bool collected;

        public void Init() {
            collected = false;
        }

        public XmlNode Save(XmlDocument doc) {
            XmlElement item = doc.CreateElement(id);
            item.SetAttribute("collected", Convert.ToString(collected));
            return item;
        }

        public void Load(XmlElement itemInfo) {
            if (itemInfo == null) return;

            var collectedValue = itemInfo.GetAttribute("collected");
            if(collectedValue != null)
                collected = Convert.ToBoolean(collectedValue);
        }
    }
}