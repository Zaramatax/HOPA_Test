using UnityEngine;
using System.Collections;
using System;

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
	}
}