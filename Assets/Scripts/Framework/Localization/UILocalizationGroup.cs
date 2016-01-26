using UnityEngine;
using System.Collections;
using System;

namespace Framework {
    public class UILocalizationGroup : MonoBehaviour {
        public string preffix;

        public static UILocalizationGroup Find (GameObject gameObject) {
            var cur = gameObject.transform;
            while (cur.parent != null && cur.GetComponent<UILocalizationGroup>() == null) {
                cur = cur.parent;
            }
            return cur.GetComponent<UILocalizationGroup>();

        }
    }
}