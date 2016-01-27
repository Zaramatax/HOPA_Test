using UnityEngine;
using UnityEngine.UI;
using System;

namespace Framework {
    public class DialogueManager : MonoBehaviour {

        private static string dialoguesPath = "Prefabs/Dialogues/";

        public BlackStripes blackStripes;
        public Text textField;

        public static DialogueManager instance;

        private Dialogue activeDialog;
        private Action onComplete;
        private bool isActive;

        void Awake() {
            isActive = false;
        }

        void Start() {
            if(this != instance) {
                instance = this;
            }

            blackStripes.onShow += OnBlackStripesShow;
            blackStripes.onHide += OnBlackStripesHide;
        }

        void OnDestroy() {
            blackStripes.onShow -= OnBlackStripesShow;
            blackStripes.onHide -= OnBlackStripesHide;
        }

        void Update() {
            if(Input.GetMouseButtonDown(0) && isActive) {
                ShowNext();
            }
        }

        public void Show(string dialogName, Action onComplete) {
            Dialogue dialog = Resources.Load<Dialogue>(dialoguesPath + dialogName);
            activeDialog = Instantiate(dialog, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as Dialogue;
            activeDialog.transform.SetParent(transform);
            
            activeDialog.onComplete += Complete;

            this.onComplete = onComplete;

            isActive = true;

            blackStripes.Show();
        }

        void Complete() {
            Destroy(activeDialog);

            isActive = false;

            blackStripes.Hide();
        }

        void OnBlackStripesShow() {
            activeDialog.SetTextField(textField);
            ShowNext();
        }

        void OnBlackStripesHide() {
            if(onComplete != null) {
                onComplete();
            }
        }

        void ShowNext() {
            if(activeDialog) {
                activeDialog.ShowNext();
            }
        }
    }
}