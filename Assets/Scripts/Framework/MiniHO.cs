using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework {
    public class MiniHO : SubLocation {

        [SerializeField]
        public List<MiniHOItem> itemsOnScene;
        public List<MiniHOItem> itemsOnPlace;

        private Dictionary<MiniHOItem, MiniHOItem> items;

        private CanvasGroup canvasGroup;

        protected override void Awake() {
            base.Awake();
            canvasGroup = GetComponent<CanvasGroup>();

            if (itemsOnScene.Count != itemsOnPlace.Count) throw new Exception("items on scene count != items on place count : on scene " + name);

            items = new Dictionary<MiniHOItem, MiniHOItem>();
            for(int i = 0; i < itemsOnScene.Count; i++) {
                itemsOnScene[i].ItemOnPlace += OnItemOnPlace;
                items.Add(itemsOnScene[i], itemsOnPlace[i]);
            }
        }

        protected override void OnGameObjectClicked(GameObject go) {
            var clickedItem = go.GetComponent<MiniHOItem>();
            if (clickedItem == null) return;

            if (!items.ContainsKey(clickedItem)) return;

            var targetItem = items[clickedItem];
            clickedItem.OnClick(targetItem.transform.position);
        }

        public void OnItemOnPlace(object sender, EventArgs e) {
            var itemOnScene = (MiniHOItem)sender;

            if (!items.ContainsKey(itemOnScene)) return;
            var onPlaceItem = items[itemOnScene];
            onPlaceItem.OnItemOnPlace();
        }
    }
}
