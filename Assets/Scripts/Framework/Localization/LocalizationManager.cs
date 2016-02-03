using UnityEngine;
using System.Collections;
using System;

namespace Framework {
    public class LocalizationManager : MonoBehaviour {
        public static LocalizationManager instance;

        void Awake () {
            instance = this;
            SwitchLocale(PlayerPrefs.GetString("locale"));
        }

        public string localesPath = "Localization";

        public Localization current;

        public static event System.Action OnLocaleSwitched;

        public void SwitchLocale (string name) {
            var loc = Resources.Load<Localization>(localesPath + "/" + name);
            if (loc != null) {
                current = loc;
                if (OnLocaleSwitched != null)
                    OnLocaleSwitched();
            }
            PlayerPrefs.SetString("locale", name);
        }

        public static string GetTranslationStatic (string key, MonoBehaviour context = null) {
            if (instance == null)
                return key;
            return instance.GetTranslation(Localization.ParseKey(key, context));
        }

        public string GetTranslation (string key) {
            if (current == null) {
                var all = Resources.LoadAll<Localization>(localesPath);
                if (all.Length > 0)
                    current = all[0];
            }

            if (current != null)
                return current.GetTranslation(key);

            return key;
        }
    }
}