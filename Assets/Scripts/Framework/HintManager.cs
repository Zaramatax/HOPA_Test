using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Framework {
    public class HintInfo {
        public GameObject Layer { set; get; }
        public InventoryItem Item { set; get; }

        private HintInfo(GameObject layer, InventoryItem item) {
            Layer = layer;
            Item = item;
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
            if (info.Layer) {
                hintEffectLayer.transform.position = info.Layer.transform.position;
                hintEffectLayer.Play("show_hint", -1, 0f);
            }

            if (info.Item) {
                hintEffectInventory.transform.position = InventoryManager.instance.GetItemPosition(info.Item);
                hintEffectInventory.Play("show_hint", -1, 0f);
            }
        }
    }
}