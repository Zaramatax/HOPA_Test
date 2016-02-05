using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Framework {
    public class CollectionContainer : MonoBehaviour {
        public const string textPath = "gameplay/collections";

        public Transform content;
        public GameObject itemPrefab;

        public Text collectionName;
        public Image icon;

        private List<CollectionItemContainer> itemsInst;

        void Awake() {
            itemsInst = new List<CollectionItemContainer>();
        }

        public void Setup(Collection collection) {
            collectionName.text = LocalizationManager.GetTranslationStatic(collection.Title);
            icon.sprite = collection.Icon;

            for(int i = 0; i < collection.ItemsCount; i++) {
                var go = GameObject.Instantiate<GameObject>(itemPrefab);
                go.transform.SetParent(content.transform, false);
                var collectionItemContainer = go.GetComponent<CollectionItemContainer>();
                collectionItemContainer.Setup(collection.GetItem(i));
                itemsInst.Add(collectionItemContainer);
            }
        }

        public void RefreshDisplay() {
            itemsInst.ForEach(x => x.RefreshDisplay());
        }
    }
}
