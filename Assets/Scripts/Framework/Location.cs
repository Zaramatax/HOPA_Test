using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Xml;

namespace Framework {
    public class Location : MonoBehaviour {
        protected InventoryManager _inventory;
        protected List<Timer> timers;

        public LayerMask layerMask;
        public string locationName;

        virtual protected void OnGameObjectClicked(GameObject layer) { }
        virtual protected void Cheat() { }
        virtual protected void CreateTimers() { }

        virtual protected void Awake() {
            
        }

        virtual protected void Start() {
            _inventory = InventoryManager.instance;
            timers = new List<Timer>();

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

            UpdateTimers(Time.deltaTime);
        }

        protected Timer CreateTimer(string name, float time, System.Action action) {
            timers.Add(new Timer(name, time, action));

            return timers[timers.Count - 1];
        }

        void Save() {
            XmlDocument doc = new XmlDocument();
            XmlNode root = doc.CreateElement("root");
            doc.AppendChild(root);

            SaveLocationState(doc);
            SaveTimers(doc);

            doc.Save(locationName + ".xml");
        }

        void Load() {
            XmlDocument doc = new XmlDocument();
            try {
                doc.Load(locationName + ".xml");

                LoadLocationState(doc);
                LoadTimers(doc);
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

        void UpdateTimers(float delta) {
            for (int i = 0; i < timers.Count; i++)
                timers[i].Update(delta);
        }

        void SaveTimers(XmlDocument doc) {
            XmlNode timersNode = doc.CreateElement("timers");
            doc.DocumentElement.AppendChild(timersNode);

            for (int i = 0; i < timers.Count; i++)
                timers[i].SaveToXML(timersNode, doc);
        }

        void LoadTimers(XmlDocument doc) {
            XmlNode timersNode = doc.DocumentElement.SelectSingleNode("timers");

            for (int i = 0; i < timers.Count; i++)
                timers[i].LoadFromXML(timersNode);
        }
    }
}