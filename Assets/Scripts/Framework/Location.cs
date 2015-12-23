using UnityEngine;
using System.Collections;
using System.Xml;

namespace Framework
{
   public class Location : MonoBehaviour
    {
        protected InventoryManager _inventory;

        public LayerMask layerMask;
        public string locationName;

		virtual public void OnGameObjectClicked(GameObject layer) {}
        virtual public void Cheat() {}

        virtual protected void Awake() {}

        virtual protected void Start()
        {
            _inventory = InventoryManager.instance;

            Load();

            Cheat();
        }

        virtual protected void OnDestroy() {
            Save();
        }

        virtual protected void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, Mathf.Infinity, layerMask);

                if (hit.collider)
                {
					OnGameObjectClicked(hit.collider.gameObject);
                }
            }
        }

       void Save() {
           XmlDocument doc = new XmlDocument();
           XmlNode root = doc.CreateElement("root");
           doc.AppendChild(root);

           LocationState.SaveToXML(transform, root, doc);

           doc.Save(locationName + ".xml");
       }

       void Load() {
           XmlDocument doc = new XmlDocument();
           try {
               doc.Load(locationName + ".xml");
               XmlElement root = doc.DocumentElement;
               LocationState.LoadFromXML(transform, root, doc);
           }
           catch { };
       }
    }
}