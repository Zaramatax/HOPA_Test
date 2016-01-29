using UnityEngine;
using System.Xml;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Framework {
    public class Location : MonoBehaviour, IPointerClickHandler {
		
        protected InventoryManager inventory;
		protected TimerManager timerManager;
        protected DialoguesOnScene dialogues;

        public LayerMask layerMask;
        public List<SubLocation> subLocations;

        virtual protected void OnGameObjectClicked(GameObject layer) { }
        virtual protected void Cheat() { }
        virtual protected void CreateTimers() { }
        virtual protected void CreateDialogues() { }
        virtual protected void AddTransferZone(List<HintInfo> result) { }
        virtual protected void AddCustomHints(List<HintInfo> result) { }

        protected string locationName;

        virtual protected void Awake() {
			timerManager = new TimerManager ();
            dialogues = new DialoguesOnScene();
        }

        virtual protected void Start() {

            inventory = InventoryManager.instance;
            locationName = gameObject.name;

            foreach(SubLocation sub in subLocations) {
                sub.gameObject.SetActive(false);
            }

            CreateTimers();
            CreateDialogues();
            Load();
            Cheat();
        }

        virtual protected void OnDestroy() {
            Save();
        }

        public void OnPointerClick (PointerEventData eventData) {
            OnGameObjectClicked(eventData.rawPointerPress);
        }

        virtual protected void Update() {
			timerManager.UpdateTimers(Time.deltaTime);
        }

        virtual protected void Save() {
            XmlDocument doc = new XmlDocument();
            XmlNode root = doc.CreateElement("root");
            doc.AppendChild(root);

            SaveLocationState(doc);
			timerManager.Save(doc);
            dialogues.Save();
            
            ProfileSaver.Save(doc, locationName);
        }

        virtual protected void Load() {
            XmlDocument doc = new XmlDocument();

            if (ProfileSaver.Load(doc, locationName)) {
                LoadLocationState(doc);
				timerManager.Load(doc);
                dialogues.Load(locationName);
            }
        }

        void SaveLocationState(XmlDocument doc) {
            XmlNode locationState = doc.CreateElement("location_state");
            doc.DocumentElement.AppendChild(locationState);
            LocationState.SaveToXML(transform, locationState, doc);
        }

        void LoadLocationState(XmlDocument doc) {
            XmlNode locationState = doc.DocumentElement.SelectSingleNode("location_state");
            LocationState.LoadFromXML(transform, locationState, doc);
        }

        protected List<HintInfo> CreateHints() {
            List<HintInfo> result = new List<HintInfo>();

            AddDropZones(result);
            AddPickItems(result);
            AddCustomHints(result);
            AddSublocations(result);

            return result;
        }

        void AddSublocations(List<HintInfo> result) {
            if (result.Count == 0) {
                foreach (SubLocation sub in subLocations) {
                    if (sub.CreateHints().Count > 0) {
                        GameObject openSub = GetOpenSubZone(sub);
                        result.Add(HintInfo.CreateHint(openSub));
                    }
                }
            }
        }

        GameObject GetOpenSubZone(SubLocation sub) {
            foreach (Transform child in transform) {
                OpenSubArea openSub = child.gameObject.GetComponent<OpenSubArea>();
                if (openSub != null && child.gameObject.activeInHierarchy && sub == openSub.subLocation) {
                    return child.gameObject;
                }
            }

            return null;
        }

        public List<HintInfo> GetHints() {
           foreach (SubLocation sub in subLocations) {
                if (sub.IsOpen()) {
                    return sub.GetHints();
                }
            }

            List<HintInfo> result = CreateHints();

            AddTransferZone(result);

            return result;
        }

        void AddDropZones(List<HintInfo> result) {
            foreach (DropZone zone in transform.GetComponentsInChildren<DropZone>()) {
                InventoryItem item = inventory.GetItem(zone.requiredItem.itemId);

                if (inventory.GetItemsCount(item.itemId) == zone.itemsCount) {
                    result.Add(HintInfo.CreateHint(zone.gameObject, item));
                }
            }
        }
        
        void AddPickItems(List<HintInfo> result) {
            foreach (PickZone zone in transform.GetComponentsInChildren<PickZone>()) {
                if (zone.gameObject.activeInHierarchy) {
                    result.Add(HintInfo.CreateHint(zone.gameObject));
                }
            }
        }

        public string GetName() {
            return locationName;
        }

        protected void StartDialogue (string name) {
            StartDialogue(dialogues.GetDialogue(name));
        }

        protected void StartDialogue(DialogueEvent dialogue) {
            DialogueManager.instance.Show(dialogue);
        }
    }
}