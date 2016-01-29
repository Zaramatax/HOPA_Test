using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;

namespace Framework {
    public class DialogueManager : MonoBehaviour, IPointerClickHandler {

        private static string dialoguesPath = "Prefabs/Dialogues/";
        private static string showInventory = "open";
        private static string hideInventory = "close";

        public BlackStripes blackStripes;
        public Text textField;

        public static DialogueManager instance;

        private Dialogue activeDialogue;
        private Action onComplete;
        private bool isDialogueActive;
        private GameObject inventoryPanelGO;

        void Awake() {
            isDialogueActive = false;
        }

        void Start() {
            if(this != instance) {
                instance = this;
            }

            blackStripes.onShow += OnBlackStripesShow;
            blackStripes.onHide += OnBlackStripesHide;

            inventoryPanelGO = GameObject.Find("BottomPanel");

            Assert.IsNotNull(textField, "Error: dialogue text field not found");
        }

        void OnDestroy() {
            blackStripes.onShow -= OnBlackStripesShow;
            blackStripes.onHide -= OnBlackStripesHide;
        }

        public void OnPointerClick (PointerEventData eventData) {
            if(isDialogueActive) {
                ShowNext();
            }
        }

        public void Show(string dialogueName, Action onComplete) {
            Dialogue dialogue = Resources.Load<Dialogue>(dialoguesPath + dialogueName);

            if (dialogue) {
                activeDialogue = Instantiate(dialogue, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as Dialogue;
                activeDialogue.transform.SetParent(transform);

                activeDialogue.onComplete += Complete;

                this.onComplete = onComplete;

                textField.gameObject.SetActive(true);
                isDialogueActive = true;

                blackStripes.Show();

                if(inventoryPanelGO) {
                    inventoryPanelGO.GetComponent<Animator>().Play(hideInventory);
                }
            }
        }

        void Complete() {
            Destroy(activeDialogue.gameObject);

            textField.gameObject.SetActive(false);
            isDialogueActive = false;

            blackStripes.Hide();

            if (inventoryPanelGO) {
                inventoryPanelGO.GetComponent<Animator>().Play(showInventory);
            }
        }

        void OnBlackStripesShow() {
            activeDialogue.SetTextField(textField);
            ShowNext();
        }

        void OnBlackStripesHide() {
            if(onComplete != null) {
                onComplete();
            }
        }

        void ShowNext() {
            if(activeDialogue) {
                activeDialogue.ShowNext();
            }
        }
    }
}