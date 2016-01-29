using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;
using System.Xml;

namespace Framework {
    public class DialogueManager : MonoBehaviour, IPointerClickHandler {

        private const string dialoguesPath = "Prefabs/Dialogues/";
        private const string showInventory = "open";
        private const string hideInventory = "close";
        private const string saveDocument = "dialogues";

        public BlackStripes blackStripes;
        public Text textField;

        public static DialogueManager instance;

        private Dialogue activeDialogue;
        private Action onComplete;
        private bool isDialogueActive;
        private GameObject inventoryPanelGO;

        private XmlNode loadNode;

        void Awake() {
            isDialogueActive = false;
            loadNode = null;
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
            SaveActiveDialogue();

            blackStripes.onShow -= OnBlackStripesShow;
            blackStripes.onHide -= OnBlackStripesHide;
        }

        public void OnPointerClick (PointerEventData eventData) {
            if(isDialogueActive) {
                ShowNext();
            }
        }

        public void Show(DialogueEvent dialogue) {
            Show(dialogue.Name, dialogue.DialogueAction);
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
            activeDialogue.LoadFromXML(loadNode);
            ShowNext();
            loadNode = null;
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

        public void SaveActiveDialogue() {
            if(activeDialogue) {
                XmlDocument doc = new XmlDocument();
                XmlNode root;

                if(ProfileSaver.Load(doc, saveDocument)) {
                    root = doc.DocumentElement;
                }
                else {
                    root = doc.CreateElement("dialogues");
                    doc.AppendChild(root);
                }
                
                activeDialogue.SaveToXML(root, doc);

                ProfileSaver.Save(doc, saveDocument);
            }
        }

        public void LoadActiveDialogue(DialogueEvent dialogue, XmlNode node) {
            Show(dialogue);
            loadNode = node;
        }
    }
}