using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Framework {
    public abstract class UILocalizationBase : MonoBehaviour {
        UILocalizationGroup group;

        protected virtual void Start () {
            group = UILocalizationGroup.Find(gameObject);

            LocalizationManager.OnLocaleSwitched += translate;

            translate();
        }

        void OnDestroy () {
            LocalizationManager.OnLocaleSwitched -= translate;
        }

        protected abstract string TranslationKey { get; }

        protected virtual void translate () {
            GetComponent<Text>().text = LocalizationManager.instance.GetTranslation((group != null ? group.preffix + "/" : "") + TranslationKey);
        }
    }
}