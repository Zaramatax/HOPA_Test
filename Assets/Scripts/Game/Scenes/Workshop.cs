using UnityEngine;
using System.Collections;

using Framework;

namespace Scenes {
    public class Workshop : Location {

        override protected void Start() {
            base.Start();
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
    }
}