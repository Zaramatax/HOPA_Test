using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;

namespace Framework {
    public class Location : MonoBehaviour {
		
        protected InventoryManager _inventory;
		protected TimerManager timerManager;

        public LayerMask layerMask;
        public List<SubLocation> subLocations;

        virtual protected void OnGameObjectClicked(GameObject layer) { }
        virtual protected void Cheat() { }
        virtual protected void CreateTimers() { }
        virtual protected void AddTransferZone(ref List<HintInfo> result) { }
        virtual protected void AddCustom(ref List<HintInfo> result) { }

        protected string locationName;

        virtual protected void Awake() {
			timerManager = new TimerManager ();
        }

        virtual protected void Start() {
            _inventory = InventoryManager.instance;
            locationName = gameObject.name;

            CreateTimers();
            Load();
            Cheat();
        }

        virtual protected void OnDestroy() {
            Save();
        }

        virtual protected void Update() {
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, Mathf.Infinity, layerMask);

                if (hit.collider) {
                    OnGameObjectClicked(hit.collider.gameObject);
                }
            }

			timerManager.UpdateTimers(Time.deltaTime);
        }

        virtual protected void Save() {
            XmlDocument doc = new XmlDocument();
            XmlNode root = doc.CreateElement("root");
            doc.AppendChild(root);

            SaveLocationState(doc);
			timerManager.Save(doc);

            doc.Save(locationName + ".xml");
        }

        virtual protected void Load() {
            XmlDocument doc = new XmlDocument();
            try {
                doc.Load(locationName + ".xml");

                LoadLocationState(doc);
				timerManager.Load(doc);
            }
            catch { };
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

            AddDropZones(ref result);
            AddPickItems(ref result);
            AddCustom(ref result);
            AddSublocations(ref result);

            return result;
        }

        void AddSublocations(ref List<HintInfo> result) {
            if (result.Count == 0) {
                foreach (SubLocation sub in subLocations) {
                    if (sub.CreateHints().Count > 0) {
                        GameObject openSub = GetOpenSubZone(sub);
                        if (openSub) {
                            result.Add(HintInfo.CreateHint(openSub));
                        }
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

            AddTransferZone(ref result);

            return result;
        }

        void AddDropZones(ref List<HintInfo> result) {
            foreach (DropZone zone in transform.GetComponentsInChildren<DropZone>()) {
                InventoryItem item = _inventory.GetItem(zone.requiredItem.itemId);

                if (_inventory.GetItemsCount(item.itemId) == zone.itemsCount) {
                    result.Add(HintInfo.CreateHint(zone.gameObject, item));
                }
            }
        }
        
        void AddPickItems(ref List<HintInfo> result) {
            //TODO: add pick items
        }

        public string GetName() {
            return locationName;
        }
    }
}