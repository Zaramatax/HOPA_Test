using UnityEngine;
using System.Collections;

using Framework;

namespace Scenes {
    public class Workshop : Location {

        override protected void Start() {
            base.Start();

            DialogueManager.instance.Show("dialogue_1", AfterDialog1);
        }

        private void AfterDialog1() {
            Utils.HideGameObjects(transform, "blue_nugget");
        }
    }
}