using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Framework {
    public class HintInfo {
        public GameObject layer = null;
        public InventoryItem item = null;

        private HintInfo(GameObject layer, InventoryItem item) {
            this.layer = layer;
            this.item = item;
        }

        public static HintInfo CreateHint(GameObject layer, InventoryItem item) {
            return new HintInfo(layer, item);
        }

        public static HintInfo CreateHint(GameObject layer) {
            return new HintInfo(layer, null);
        }
    }

    public class HintManager : MonoBehaviour {
        public Animator hintEffectLayer;
        public Animator hintEffectInventory;

        private LocationManager locationManager;

        void Start() {
            locationManager = LocationManager.instance;
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.H)) {
                List<HintInfo> info = locationManager.GetCurrentLocation().GetHints();

                if (info.Count > 0) {
                    int index = Random.Range(0, info.Count);
                    CreateHintEffect(info[index]);
                }
                else {
                }
            }
        }

        void CreateHintEffect(HintInfo info) {
            hintEffectLayer.transform.position = info.layer.transform.position;
            hintEffectLayer.Play("show_hint", -1, 0f);

            if (info.item) {
                hintEffectInventory.transform.position = InventoryManager.instance.GetItemPosition(info.item);
                hintEffectInventory.Play("show_hint", -1, 0f);
            }
        }
    }
}