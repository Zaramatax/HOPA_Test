using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace Framework {
    public class Dialogue : MonoBehaviour {
        private List<DialogueStage> stages;
        private int currentStage;
        public Action onComplete;

        void Awake() {
            stages = new List<DialogueStage>();
            currentStage = -1;
        }

        void Start() {
            foreach(Transform stageTransform in transform) {
                DialogueStage stage = stageTransform.gameObject.GetComponent<DialogueStage>();
                stage.onStageComplete += OnStageCompleted;
                stages.Add(stage);
            }
        }

        void OnDestroy() {
            foreach(DialogueStage stage in stages) {
                stage.onStageComplete -= OnStageCompleted;
            }
        }

        public void SetTextField(Text textField) {
            foreach(DialogueStage stage in stages) {
                stage.SetTextField(textField);
            }
        }

        public void ShowNext() {
            if(currentStage < 0) {
                ShowNextStage();
                return;
            }

            stages[currentStage].ShowNext();
        }

        private void ShowNextStage() {
            ++currentStage;

            if (currentStage == stages.Count) {
                onComplete();
                return;
            }

            stages[currentStage].Show();

            if(currentStage > 0) {
                stages[currentStage - 1].Hide();
            }
        }

        private void OnStageCompleted() {
            ShowNextStage();
        }
    }
}