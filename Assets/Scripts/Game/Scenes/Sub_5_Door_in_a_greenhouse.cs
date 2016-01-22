using UnityEngine;
using System.Collections;
using System;

using Framework;

namespace Scenes
{
	public class Sub_5_Door_in_a_greenhouse : SubLocation {

		public void OnUseFlint()
		{
            Utils.ShowGameObjects(thisSub, "Flint", "place_flame_embem");
			Utils.GameObjectSetActive (thisSub, "place_flint", false);
		}

		public void OnUseFlameEmblem()
		{
			Utils.GameObjectSetActive (thisSub, "ogon", true);
			Utils.GameObjectSetActive (thisSub, "place_flame_embem", false);
			StartCoroutine ("Flame");
		}

		IEnumerator Flame()
		{
			yield return new WaitForSeconds (2);

            Utils.ShowGameObjects(thisSub, "After fire");
            Utils.HideGameObjects(thisSub, "ogon", "Before fire", "Flint");

            Close();

			Utils.ShowGameObjects(mainLocation, "glass_door_open", "go_to_location");
            Utils.HideGameObjects(mainLocation, "open_sub_5");
		}

	}
}