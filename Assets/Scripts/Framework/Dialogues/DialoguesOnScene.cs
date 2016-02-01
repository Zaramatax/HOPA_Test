using System.Collections.Generic;
using System;
using System.Xml;

namespace Framework {
    public class DialogueEvent {
        public string Name { get; set; }
        public Action DialogueAction { get; set; }

        public DialogueEvent(string name, Action action) {
            Name = name;
            DialogueAction = action;
        }
    }

    public class DialoguesOnScene {
        List<DialogueEvent> dialogues;
        private const string saveDocument = "dialogues";

        private const string dialoguesNodeName = "dialogue";

        public DialoguesOnScene() {
            dialogues = new List<DialogueEvent>();
        }

        public DialogueEvent CreateDialogue (string name, Action action) {
            dialogues.Add(new DialogueEvent(name, action));
            return dialogues[dialogues.Count - 1];
        }

        public DialogueEvent GetDialogue(string name) {
            return dialogues.Find(x => x.Name == name);
        }

        public void Save () {
            DialogueManager.instance.SaveActiveDialogue();
        }

        public void Load (string location) {
            XmlDocument doc = new XmlDocument();

            if(ProfileSaver.Load(doc, saveDocument)) {
                XmlNode dialogueNode = doc.DocumentElement.SelectSingleNode(location);
                if (dialogueNode != null) {
                    string dialogueName = dialogueNode.Attributes["name"].Value;
                    DialogueManager.instance.LoadActiveDialogue(GetDialogue(dialogueName), dialogueNode);
                }
            }
        }
    }
}