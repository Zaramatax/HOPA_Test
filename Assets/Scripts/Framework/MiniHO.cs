using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework {
    public class MiniHO : SubLocation {

        public List<MiniHOItem> items;
        public event Action<MiniHO> MiniHOComplete;

        protected override void Awake() {
            base.Awake();

            foreach (MiniHOItem item in items) {
                item.Init();
                item.MoveComplete += OnItemMoveComplete;
            }
        }

        protected override void OnApplicationQuit() {
            base.OnDestroy();
            items.ForEach(x => x.MoveComplete -= OnItemMoveComplete);
        }

        protected override void Save() {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("root");
            doc.AppendChild(root);

            for(int i = 0; i < items.Count; i++) {
                root.AppendChild(items[i].Save(doc));
            }

            ProfileSaver.Save(doc, locationName);
        }

        protected override void Load() {
            XmlDocument doc = new XmlDocument();
            if (ProfileSaver.Load(doc, locationName)) {
                XmlElement root = doc.DocumentElement;
                for(int i = 0; i < root.ChildNodes.Count; i++) {
                    items[i].Load((XmlElement)root.ChildNodes[i]);
                }
            }

            CheckComplete();
        }

        protected override void OnGameObjectClicked(GameObject go) {
            var item = go.GetComponent<MiniHOItem>();
            if (item == null) return;
            OnItemClick(item);
        }

        public void OnItemMoveComplete(MiniHOItem item) {
            OnItemPlace(item);
        }

        public void FadeShow(GameObject item){
            if (item == null) return;

            var animator = item.GetComponent<Animator>();
            if (animator == null) return;

            item.SetActive(true);
            animator.Play(fadeShow);
        }

        public void FadeHide(GameObject item){
            if (item == null) return;

            var animator = item.GetComponent<Animator>();
            if (animator == null) return;

            animator.Play(fadeHide);
            StartCoroutine(SetActive(item, false));
        }

        public virtual void OnItemClick(MiniHOItem item) {
            Disable();
            FadeHide(item.shadow);
            if (item.patch != null) {
                item.patch.SetActive(false);
            }
            item.Fly(item.place.transform.position);
        }

        public virtual void OnItemPlace(MiniHOItem item) {
            Enable();
            FadeHide(item.gameObject);
            FadeShow(item.place.gameObject);
            FadeShow(item.shadow);

            CheckComplete();
        }

        public void CheckComplete() {
            foreach(MiniHOItem item in items)
                if (!item.collect) return;

            if (MiniHOComplete != null)
                MiniHOComplete(this);

            Disable();
            StartCoroutine(CloseMiniHO());
        }

        IEnumerator CloseMiniHO() {
            yield return new WaitForSeconds(0.5f);
            Close();
        }

        IEnumerator SetActive(GameObject go, bool value) {
            yield return new WaitForSeconds(0.5f);
            go.SetActive(value);
        }
    }
}
