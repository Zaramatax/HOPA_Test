using UnityEngine;
using System.Collections;
using Framework;

namespace Scenes{
    public class SubMiniHO : MiniHO {

        public override void OnItemClick(MiniHOPair item) {
            Disable();
            FadeHide(item.onScene.shadow);
            item.onScene.Fly(item.onPlace.transform.position);
        }

        public override void OnItemPlace(MiniHOPair item) {
            Enable();
            FadeHide(item.onScene.gameObject);
            FadeHide(item.onPlace.gameObject);

            CheckComplete();
        }

    }
}
