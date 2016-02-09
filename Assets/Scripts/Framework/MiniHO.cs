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

        private const string fadeShow = "fade_show";
        private const string fadeHide = "fade_hide";

        public RuntimeAnimatorController animatorController;
        public List<MiniHOPair> items;

        public int collectedCount { get; private set; }

        protected override void Awake() {
            base.Awake();

            collectedCount = 0;

            foreach (MiniHOPair pair in items) {
                pair.onScene.Init(animatorController);
                pair.onPlace.Init(animatorController);
                pair.onScene.MoveComplete += OnItemMoveComplete;
            }
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            items.ForEach(x => x.onScene.MoveComplete -= OnItemMoveComplete);
        }

        protected override void Save() {
            base.Save();
            XmlDocument doc = new XmlDocument();

            if (ProfileSaver.Load(doc, locationName)) {
                XmlElement root = doc.DocumentElement;
                XmlElement collected = doc.CreateElement("collected");
                collected.SetAttribute("value", Convert.ToString(collectedCount));
                root.AppendChild(collected);

                ProfileSaver.Save(doc, locationName);
            }
        }

        protected override void Load() {
            base.Load();

            XmlDocument doc = new XmlDocument();

            if (ProfileSaver.Load(doc, locationName)) {
                XmlElement root = doc.DocumentElement;
                XmlElement collected = Utils.GetElement(root, "collected");
                collectedCount = Convert.ToInt32(Utils.GetAttribute(collected, "value"));
            }
        }

        protected override void OnGameObjectClicked(GameObject go) {
            var clickedItem = go.GetComponent<MiniHOItem>();
            if (clickedItem == null) return;

            var miniHOPair = items.Find(x => x.onScene == clickedItem);
            if (miniHOPair == null) return;

            collectedCount++;

            OnItemClick(miniHOPair);
        }

        public void OnItemMoveComplete(MiniHOItem onSceneItem) {
            var miniHOPair = items.Find(x => x.onScene == onSceneItem);
            if (miniHOPair == null) return;

            OnItemPlace(miniHOPair);
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

        public virtual void OnItemClick(MiniHOPair item) {
            Disable();
            FadeHide(item.onScene.shadow);
            if (item.onScene.patch != null) {
                item.onScene.patch.SetActive(false);
            }
            item.onScene.Fly(item.onPlace.transform.position);
        }

        public virtual void OnItemPlace(MiniHOPair item) {
            Enable();
            FadeHide(item.onScene.gameObject);
            FadeShow(item.onPlace.gameObject);
            FadeShow(item.onPlace.shadow);
            if(item.onPlace.patch != null) {
                item.onPlace.patch.SetActive(true);
            }

            CheckComplete();
        }

        public void CheckComplete() {
            if (collectedCount == items.Count) {
                Disable();
                StartCoroutine(CloseMiniHO());
            }
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

    [Serializable]
    public class MiniHOPair {
        public MiniHOItem onScene;
        public MiniHOItem onPlace;
    }
}
