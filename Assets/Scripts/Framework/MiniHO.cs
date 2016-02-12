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

        private AnimationSystem animator = AnimationSystem.Instance;

        protected override void Awake() {
            base.Awake();
        }

        protected override void OnGameObjectClicked(GameObject go) {
            var item = go.GetComponent<MiniHOItem>();
            if (item == null) return;
            OnItemClick(item);
        }

        public void OnItemClick(MiniHOItem item) {
            Disable();
            //item.collect = true;
            if (item.shadow != null) {
                animator.Hide(item.shadow, 0.5f);
            }
            if (item.patch != null) {
                animator.InstHide(item.patch);
            }
            animator.Move(item.gameObject, item.transform.position, item.place.transform.position, 10, "OnItemPlace");
        }

        public void OnItemPlace(AnimationEventArgs e) {
            Enable();

            var miniHOItem = e.go.GetComponent<MiniHOItem>();
            animator.Hide(miniHOItem.gameObject, 0.5f);
            animator.Show(miniHOItem.place, 0.5f, "CheckComplete");
        }

        public void CheckComplete() {
            foreach (MiniHOItem item in items)
                if (item.gameObject.activeSelf) return;

            if (MiniHOComplete != null)
                MiniHOComplete(this);

            Disable();
            Close();
        }
    }
}
