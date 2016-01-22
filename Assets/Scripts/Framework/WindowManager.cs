using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Framework {
    public class WindowManager : MonoBehaviour {

        public List<GameObject> windows;
        public static WindowManager instance;
        public GameObject black;

        void Start() {
            instance = this;
        }

        public void OpenWellDoneWindow(InventoryItem item, System.Action action) {
            WellDone win = Open<WellDone>();
            win.Init(item, action);
        }

        private T Open<T>() where T: Window {
            GameObject winGO = windows.Find(x => x.GetComponent<T>() != null);

            if (winGO) {
                T window = winGO.GetComponent<T>();
                window.Open();
                black.SetActive(true);
                return window;
            }

            return null;
        }

        public void Close() {
            black.SetActive(false);
        }
    }
}