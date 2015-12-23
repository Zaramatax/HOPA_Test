using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using Framework;

namespace Scenes
{
    public class Sub_2_Door_to_house : SubLocation
    {
		public event System.Action DoorToHouseOpen;

        override public void OnGameObjectClicked(GameObject layer)
        {
            if (layer.name == "hand_feather" && _inventory.GetSelectedItem() == "hand_with_feather")
            {
				Utils.GameObjectSetActive(transform, "hand_1", true);
                _inventory.RemoveItem("hand_with_feather");
                CheckDone();
            }

            if (layer.name == "hand_feather" && _inventory.GetSelectedItem() == "blue_nugget")
			{
				Utils.GameObjectSetActive(transform, "podpis", true);
				_inventory.RemoveItem("blue_nugget");
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
    }
}