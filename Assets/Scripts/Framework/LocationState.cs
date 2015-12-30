using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System;
using System.Globalization;

namespace Framework {
    public abstract class Attribute {
        protected string attributeName;

        protected abstract void DoLoad(GameObject gameObject, XmlAttribute attribute);
        protected abstract void DoSave(GameObject gameObject, XmlAttribute attribute);

        public void Load(GameObject gameObject, XmlNode node) {
            XmlAttribute attribute = node.Attributes[attributeName];
            if (attribute == null) {
                return;
            }

            DoLoad(gameObject, attribute);
        }

        public void Save(GameObject gameObject, XmlNode node, XmlDocument doc) {
            XmlAttribute attribute = doc.CreateAttribute(attributeName);

            DoSave(gameObject, attribute);

            node.Attributes.Append(attribute);
        }
    }

    public class Activity : Attribute {

        public Activity() {
            attributeName = "active";
        }

        override protected void DoLoad(GameObject gameObject, XmlAttribute attribute) {
            if (attribute.Value == "0") {
                gameObject.SetActive(false);
            }
            else {
                gameObject.SetActive(true);
            }
        }

        override protected void DoSave(GameObject gameObject, XmlAttribute attribute) {
            if (gameObject.activeSelf) {
                attribute.Value = "1";
            }
            else {
                attribute.Value = "0";
            }
        }
    }

    public class Position : Attribute {

        public Position() {
            attributeName = "position";
        }

        override protected void DoLoad(GameObject gameObject, XmlAttribute attribute) {
            string[] posStrs = attribute.Value.Split(' ');

            if (posStrs.Length == 3) {
                Vector3 objectPosition = gameObject.GetComponent<RectTransform>().localPosition;

                float.TryParse(posStrs[0], NumberStyles.Any, CultureInfo.InvariantCulture, out objectPosition.x);
                float.TryParse(posStrs[1], NumberStyles.Any, CultureInfo.InvariantCulture, out objectPosition.y);
                float.TryParse(posStrs[2], NumberStyles.Any, CultureInfo.InvariantCulture, out objectPosition.z);

                gameObject.GetComponent<RectTransform>().localPosition = objectPosition;
            }
        }

        override protected void DoSave(GameObject gameObject, XmlAttribute attribute) {
            Vector3 position = gameObject.GetComponent<RectTransform>().localPosition;
            attribute.Value = position.x + " " + position.y + " " + position.z;
        }
    }

    public static class LocationState {
        static List<Attribute> attributes = new List<Attribute>();

        public static void CreateSavesList() {
            attributes.Add(new Activity());
            attributes.Add(new Position());
        }

        public static void LoadFromXML(Transform parent_transform, XmlNode parent_node, XmlDocument doc) {
            foreach (Transform child in parent_transform) {

                XmlNode node = parent_node.SelectSingleNode(child.name);
                if (node != null) {
                    foreach (Attribute attribute in attributes) {
                        attribute.Load(child.gameObject, node);
                    }

                    LoadFromXML(child, node, doc);
                }
            }
        }

        public static void SaveToXML(Transform parent_transform, XmlNode parent_node, XmlDocument doc) {
            foreach (Transform child in parent_transform) {
                XmlNode node = doc.CreateElement(child.name);

                foreach (Attribute attribute in attributes) {
                    attribute.Save(child.gameObject, node, doc);
                }

                parent_node.AppendChild(node);

                SaveToXML(child, node, doc);
            }
        }
    }
}