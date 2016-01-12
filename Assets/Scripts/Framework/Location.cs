using UnityEngine;
using System.Collections;
using System.Xml;

namespace Framework {
    public class Location : MonoBehaviour {
		
        protected InventoryManager _inventory;
		protected TimerManager timerManager;

        public LayerMask layerMask;
        public string locationName;

        virtual protected void OnGameObjectClicked(GameObject layer) { }
        virtual protected void Cheat() { }
        virtual protected void CreateTimers() { }

        virtual protected void Awake() {
			timerManager = new TimerManager ();
        }

        virtual protected void Start() {
            _inventory = InventoryManager.instance;

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
    }
}