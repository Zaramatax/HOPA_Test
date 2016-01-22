using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using Framework;

namespace Scenes
{
    public class Sub_2_Door_to_house : SubLocation
    {
		public event System.Action DoorToHouseOpen;

        override protected void OnGameObjectClicked(GameObject layer)
        {
            if (layer.name == "hand_feather" && inventory.GetSelectedItem() == "hand_with_feather")
            {
				Utils.GameObjectSetActive(transform, "hand_1", true);
                inventory.RemoveItem("hand_with_feather");
                CheckDone();
            }

            if (layer.name == "hand_feather" && inventory.GetSelectedItem() == "blue_nugget")
			{
				Utils.GameObjectSetActive(transform, "podpis", true);
				inventory.RemoveItem("blue_nugget");
                CheckDone();
			}
        }

        void CheckDone() {
            if (Utils.IsGameObjectActive(transform, "hand_1") && Utils.IsGameObjectActive(transform, "podpis")) {
                Utils.HideGameObjects(transform, "hand_feather");
                StartCoroutine("OnActionsDone");                
            }
        }

		IEnumerator OnActionsDone()
		{
			yield return new WaitForSeconds(0.5f);

			Close();

			if (DoorToHouseOpen != null)
				DoorToHouseOpen ();
		}

        override protected void AddCustomHints(List<HintInfo> result) {
            if (Utils.IsGameObjectActive(transform, "hand_feather") && inventory.GetItemsCount("hand_with_feather") > 0)
                result.Add(HintInfo.CreateHint(Utils.GetGameObject(transform, "hand_feather"), inventory.GetItem("hand_with_feather")));
        }
    }
}