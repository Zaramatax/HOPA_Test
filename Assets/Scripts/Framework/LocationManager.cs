using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Framework {
    public class LocationManager : MonoBehaviour {
        enum State {
            Opening,
            Closing,
            Idle
        };

        class LocationNode {
            string name;
            LocationNode parent;
            List<LocationNode> childs;
        }

        public static LocationManager instance;
        public GameObject fade;

        string toLocation;
        State state;
        Location currentLocation;

        void Awake() {
            instance = this;
            state = State.Idle;
            toLocation = "";
            currentLocation = null;
        }

        public void GoToLocation(string location) {
            if (!currentLocation || location != currentLocation.GetName()) {
                toLocation = location;
                state = State.Closing;
                fade.SetActive(true);
            }
        }

        public void LocationLoaded() {
            state = State.Opening;
        }

        void Update() {
            if (state == State.Opening) {
                FadeIn();

                if (fade.GetComponent<Image>().color.a < 0.01f) {
                    fade.GetComponent<Image>().color = Color.clear;
                    state = State.Idle;
                    fade.SetActive(false);
                    ChangeCurrentLocation();
                    toLocation = "";
                }
            }

            if (state == State.Closing) {
                FadeOut();

                if (fade.GetComponent<Image>().color.a > 0.99f) {
                    fade.GetComponent<Image>().color = Color.black;
                    state = State.Idle;

                    Application.LoadLevel("Loading");
                }
            }
        }

        void FadeOut() {
            Color currentColor = fade.GetComponent<Image>().color;
            currentColor = Color.Lerp(currentColor, Color.black, 5.0f * Time.deltaTime);
            fade.GetComponent<Image>().color = currentColor;
        }

        void FadeIn() {
            Color currentColor = fade.GetComponent<Image>().color;
            currentColor = Color.Lerp(currentColor, Color.clear, 5.0f * Time.deltaTime);
            fade.GetComponent<Image>().color = currentColor;
        }

        public string GetLocationToLoad() {
            return toLocation;
        }

        public Location GetCurrentLocation() {
            return currentLocation;
        }

        void ChangeCurrentLocation() {
            currentLocation = GameObject.Find(toLocation).GetComponent<Location>();
        }
    }
}