﻿using UnityEngine;
using System.IO;
using System;
using System.Xml;

namespace Framework 
{
	public static class Utils {

		public static GameObject GetGameObject(Transform transform, string name)
		{
            Transform result = transform.Find(name);
            if (result) {
                return result.gameObject;
            }

            return null;
		}

		public static void GameObjectSetActive(Transform transform, string name, bool active)
		{
			GameObject gameObject = GetGameObject (transform, name);

			if (gameObject) {
				gameObject.SetActive (active);
			}
		}

		public static bool IsGameObjectActive(Transform transform, string name)
		{
			GameObject gameObject = GetGameObject (transform, name);
			
			if (gameObject) {
				return gameObject.activeInHierarchy;
			} else
				return false;
		}

        public static void ShowGameObjects(Transform transform, params string[] gameObjectNames)
        {
            foreach (string gameObjectName in gameObjectNames) {
                GameObjectSetActive(transform, gameObjectName, true);
            }
        }

		public static void HideGameObjects(Transform transform, params string[] gameObjectNames)
		{
			foreach (string gameObjectName in gameObjectNames) {
				GameObjectSetActive(transform, gameObjectName, false);
			}
		}

        public static T ParseEnum<T>(string value) {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static string GetGameObjectFullName(GameObject go) {
            string fullName = go.name;
            var parent = go.transform.parent;

            while(parent != null) {
                fullName = fullName.Insert(0, parent.name + "/");
                parent = parent.parent;
            }

            return fullName;
        }


        // for use with xml documents
        public static XmlNode GetDocRootNode(string pathName) {
            XmlDocument doc = new XmlDocument();
            doc.Load(pathName);

            return doc.DocumentElement;
        }

        public static string GetAttribute(XmlNode node, string name) {
            if (node.Attributes.GetNamedItem(name) == null) return null;
            return node.Attributes.GetNamedItem(name).Value;
        }

        public static XmlElement GetElement(XmlNode node, string name) {
            foreach (XmlElement element in node.ChildNodes)
                if (element.Name == name) return element;

            return null;
        }

        public static Vector2 Vector2Parse(string str) {
            string[] temp = str.Substring(1, str.Length - 2).Split(',');
            float x = float.Parse(temp[0]);
            float y = float.Parse(temp[1]);
            return new Vector2(x, y);
        }
    }
}