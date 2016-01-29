using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace Framework {
    [System.Serializable]
    public class Phrase {
        [LocalizationString]        
        public string message;

        public float time;
    }

    public class DialogueStage : MonoBehaviour {
        private const string showStage = "show_dialogue_stage";
        private const string hideStage = "hide_dialogue_stage";

        public List<Phrase> phrases;
        public Action onStageComplete;

        private int currentPhrase;
        private Text textField;

        void Awake() {
            currentPhrase = -1;
        }

        public void SetTextField(Text textField) {
            this.textField = textField;
        }

        public void Show () {
            gameObject.GetComponent<Animator>().Play(showStage);

            ShowNext();
        }

        public void Hide() {
            gameObject.GetComponent<Animator>().Play(hideStage);
        }

        public void ShowNext() {
            ++currentPhrase;
            
            if (currentPhrase == phrases.Count) {
                if(onStageComplete != null) {
                    onStageComplete();
                    return;
                }
            }

            textField.text = phrases[currentPhrase].message;
        }
    }
}