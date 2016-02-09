using UnityEngine;
using System.Collections;

using Framework;

namespace Scenes {
    public class Workshop : Location {

        override protected void Start() {
            base.Start();

            subLocations.Find(x => x.GetComponent<MiniHO>() != null).GetComponent<MiniHO>().MiniHOComplete += OnMiniHOComplete;
        }

        override protected void CreateDialogues () {
            dialogues.CreateDialogue("dialogue_1", AfterDialog1);
        }

        private void AfterDialog1() {
            Utils.HideGameObjects(transform, "blue_nugget");
        }

        void Update() {
            base.Update();

            if(Input.GetKeyDown(KeyCode.D)) {
                StartDialogue("dialogue_1");
            }
        }

        public void OnMiniHOComplete(MiniHO miniHO) {
            var openSubAreas = GetComponentsInChildren<OpenSubArea>();
            for(int i = 0; i < openSubAreas.Length; i++) {
                if(openSubAreas[i].subLocation == miniHO) {
                    openSubAreas[i].gameObject.SetActive(false);
                    return;
                }
            }
        }
    }
}