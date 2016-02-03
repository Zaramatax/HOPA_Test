using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

namespace Framework {
    public class Collection : MonoBehaviour {
        public string id;
        public Sprite icon;
        public List<CollectionItem> items;

        public XmlNode Save(XmlDocument doc) {
            XmlElement collection = doc.CreateElement(id);
            items.ForEach(x => collection.AppendChild(x.Save(doc)));
            return collection;
        }
    }

    [Serializable]
    public class CollectionItem {
        public string id;
        public Sprite icon;

        [HideInInspector]
        public bool collected;

        public XmlNode Save(XmlDocument doc) {
            XmlElement item = doc.CreateElement(id);
            item.SetAttribute("collected", Convert.ToString(collected));
            return item;
        }
    }
}