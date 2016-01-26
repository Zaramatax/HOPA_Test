using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace Framework {
    public class UILocalization : UILocalizationBase {
        [LocalizationString()]
        public string key;

        protected override string TranslationKey {
            get {
                return key;
            }
        }
    }
}