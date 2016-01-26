using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Framework {
    [RequireComponent(typeof(Dropdown))]
    public class UISelectLocale : MonoBehaviour {
        List<Localization> loaded = new List<Localization>();

        void Start () {
            var drop = GetComponent<Dropdown>();
            loaded.AddRange(Resources.LoadAll<Localization>(LocalizationManager.instance.localesPath));
            foreach (var locale in loaded) {
                drop.options.Add(new Dropdown.OptionData() { text = locale.name });
                if (locale == LocalizationManager.instance.current)
                    drop.value = drop.options.Count - 1;
            }

            drop.value = drop.value;
        }

        public void OnValueChanged (int val) {
            LocalizationManager.instance.SwitchLocale(loaded[val].name);
        }

        public void Select () {

            //LocalizationManager.instance.SwitchLocale(locale.name);
        }
    }
}