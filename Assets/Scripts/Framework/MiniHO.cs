using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework {
    public class MiniHO : SubLocation {

        public RuntimeAnimatorController animatorController;

        public List<MiniHOPair> items;
        private CanvasGroup canvasGroup;

        protected override void Awake() {
            base.Awake();
            canvasGroup = GetComponent<CanvasGroup>();

            foreach(MiniHOPair pair in items) {
                pair.onScene.Init(animatorController);
                pair.onPlace.Init(animatorController);
                pair.onScene.ItemOnPlace += OnItemOnPlace;
            }
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            items.ForEach(x => x.onScene.ItemOnPlace -= OnItemOnPlace);
        }

        protected override void OnGameObjectClicked(GameObject go) {
            var clickedItem = go.GetComponent<MiniHOItem>();
            if (clickedItem == null) return;

            var miniHOPair = items.Find(x => x.onScene == clickedItem);
            if (miniHOPair == null) return;

            canvasGroup.interactable = false;
            miniHOPair.onScene.OnClick(miniHOPair.onPlace.transform.position);
        }

        public void OnItemOnPlace(object sender, EventArgs e) {
            canvasGroup.interactable = true;

            var itemOnScene = (MiniHOItem)sender;

            var miniHOPair = items.Find(x => x.onScene == itemOnScene);
            if (miniHOPair == null) return;

            miniHOPair.onPlace.OnItemOnPlace();
        }
    }

    [Serializable]
    public class MiniHOPair {
        public MiniHOItem onScene;
        public MiniHOItem onPlace;
    }
}
