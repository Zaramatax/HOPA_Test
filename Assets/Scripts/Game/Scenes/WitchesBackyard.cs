using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using Framework;

namespace Scenes
{
    public class WitchesBackyard : Location
    {
		public List<SubLocation> subLocations;

		override protected void Start()
		{
			base.Start ();

			Sub_2_Door_to_house doorToHouse = (Sub_2_Door_to_house)subLocations [0];
			doorToHouse.DoorToHouseOpen += OnDoorToHouseOpen;
		}

        override protected void OnDestroy()
		{
            base.OnDestroy();

			Sub_2_Door_to_house doorToHouse = (Sub_2_Door_to_house)subLocations [0];
			doorToHouse.DoorToHouseOpen -= OnDoorToHouseOpen;
		}

        override protected void Cheat()
        {
            _inventory.AddItem("hand_with_feather");
            _inventory.AddItem("blue_nugget");
			_inventory.AddItem("flint");
			_inventory.AddItem("flame_emblem");
        }

		void OnDoorToHouseOpen()
		{
			StartCoroutine ("OnDoorToHouseOpenCoroutine");
		}

		IEnumerator OnDoorToHouseOpenCoroutine()
		{
			yield return new WaitForSeconds (0.5f);

			Utils.GameObjectSetActive (transform, "open_sub_2", false);
			Utils.GameObjectSetActive (transform, "door_open", true);
            Vector3 pos = Utils.GetGameObject(transform, "door_open").GetComponent<RectTransform>().localPosition;
            pos.x -= 20.0f;
            Utils.GetGameObject(transform, "door_open").GetComponent<RectTransform>().localPosition = pos;
		}

//----------------TEST TIMERS-------------------------------------------------

        override protected void OnGameObjectClicked(GameObject layer) {
            if (layer.name == "timer_test") {
                timers[0].Start();
            }
        }

        override protected void CreateTimers() {
            CreateTimer("test", 5, OnTimerTest);
        }

        void OnTimerTest() {
            transform.Find("back").gameObject.GetComponent<Image>().color = Color.clear;
        }
    }
}