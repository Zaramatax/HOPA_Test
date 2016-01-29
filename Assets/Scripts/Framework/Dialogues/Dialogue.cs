using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using System.Xml;

namespace Framework {
    public class Dialogue : MonoBehaviour {
        private List<DialogueStage> stages;
        private int currentStage;
        public Action onComplete;

        void Awake () {
            stages = new List<DialogueStage>();
            currentStage = -1;
        }

        void Start () {
            foreach (Transform stageTransform in transform) {
                DialogueStage stage = stageTransform.gameObject.GetComponent<DialogueStage>();
                stage.onStageComplete += OnStageCompleted;
                stages.Add(stage);
            }
        }

        void OnDestroy () {
            foreach (DialogueStage stage in stages) {
                stage.onStageComplete -= OnStageCompleted;
            }
        }

        public void SetTextField (Text textField) {
            foreach (DialogueStage stage in stages) {
                stage.SetTextField(textField);
            }
        }

        public void ShowNext () {
            if (currentStage < 0) {
                ShowNextStage();
                return;
            }

            stages[currentStage].ShowNext();
        }

        private void ShowNextStage () {
            ++currentStage;

            if (currentStage == stages.Count) {
                onComplete();
                return;
            }

            stages[currentStage].Show();

            if (currentStage > 0) {
                stages[currentStage - 1].Hide();
            }
        }

        private void OnStageCompleted () {
            ShowNextStage();
        }

        public void SaveToXML (XmlNode dialoguesNode, XmlDocument doc) {
            string location = LocationManager.instance.GetCurrentLocation().name;

            XmlNode node = doc.DocumentElement.SelectSingleNode(location);
            if (node == null) {
                node = doc.CreateElement(location);
            }

            XmlAttribute dialogueName = doc.CreateAttribute("name");
            dialogueName.Value = name.Substring(0, name.IndexOf("("));
            node.Attributes.Append(dialogueName);

            XmlAttribute stageAttr = doc.CreateAttribute("stage");
            stageAttr.Value = currentStage.ToString();
            node.Attributes.Append(stageAttr);

            stages[currentStage].SaveToXML(node, doc);

            dialoguesNode.AppendChild(node);
        }

        public void LoadFromXML (XmlNode dialogueNode) {
            if (dialogueNode != null) {
                int stage = int.Parse(dialogueNode.Attributes["stage"].Value);
                currentStage = stage - 1;
                stages[stage].LoadFromXML(dialogueNode);
            }
        }
    }
}