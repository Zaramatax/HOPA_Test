using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace Framework {
    public class UIAutoLocalization : UILocalizationBase {
        string lastText;
        string lastKey;
        Text text;

        protected override void Start () {
            text = GetComponent<Text>();
            lastKey = text.text;
            lastText = text.text;
            base.Start();
        }

        void Update () {
            if (lastText != text.text) {
                translate();
            }
        }

        protected override void translate () {
            lastKey = text.text;
            base.translate();
            lastText = text.text;
        }

        protected override string TranslationKey {
            get {
                return lastKey;
            }
        }
    }
}