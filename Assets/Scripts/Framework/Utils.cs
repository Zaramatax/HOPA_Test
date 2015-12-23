﻿using UnityEngine;
using System.Collections;
using System;

namespace Framework 
{
	public static class Utils {
		public static GameObject GetGameObject(Transform transform, string name)
		{
			return transform.Find (name).gameObject;
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
	}
}