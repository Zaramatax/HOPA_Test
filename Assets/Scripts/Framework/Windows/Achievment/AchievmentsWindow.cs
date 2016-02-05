using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Framework {
    public enum Inset { ACHIEVMENT, COLLECTIONS }

    public class AchievmentsWindow : MonoBehaviour {

        public const string showAnimName = "show_ahievment_window";
        public const string hideAnimName = "hide_ahievment_window";

        public Animator animator;

        private int showState;
        private int hideState;
        private int currentState;

        public GameObject collectionsPanel;
        public Transform collectionsContent;
        public GameObject collectionPrefab;

        public GameObject achievmentPanel;
        public Transform achievmentsContent;
        public GameObject achievmentPrefab;

        public Text score;

        [HideInInspector]
        public Inset inset;
        private List<CollectionContainer> collectionsInst;
        private List<AchievmentContainer> achievmentInst;

        void Awake() {
            collectionsInst = new List<CollectionContainer>();
            achievmentInst = new List<AchievmentContainer>();

            SetupContent();
            ChangeInset(0);
            //Hide();
        }

        void Start() {
            RewardManager.Instance.NewReward += OnNewRewardGiven;
        }

        void OnDestroy() {
            RewardManager.Instance.NewReward -= OnNewRewardGiven;
        }

        public void ShowAchievmentWindow() {
            Show();
        }

        public void ChangeInset(int insetInsex) {
            inset = (Inset)Enum.ToObject(typeof(Inset), insetInsex);

            collectionsPanel.SetActive(false);
            achievmentPanel.SetActive(false);

            switch (inset) {
                case Inset.ACHIEVMENT:
                    achievmentPanel.SetActive(true);
                    break;
                case Inset.COLLECTIONS:
                    collectionsPanel.SetActive(true);
                    break;
            }

            RefreshDisplay();
        }

        public void Show() {
            RefreshDisplay();
            animator.Play(showAnimName);
            //gameObject.SetActive(true);
        }

        public void Hide() {
            animator.Play(hideAnimName);
            //gameObject.SetActive(false);
        }

        public void OnNewRewardGiven(object reward, EventArgs e) {
            RefreshDisplay();
        }

        private void SetupContent() {
            SetupAchievments();
            SetupCollections();

            score.text = Convert.ToString(RewardManager.Instance.scorePoints);
        }

        private void SetupAchievments() {
            var achievments = RewardManager.Instance.GetAchievments();
            for (int i = 0; i < achievments.Count; i++) {
                var go = GameObject.Instantiate<GameObject>(achievmentPrefab);
                go.transform.SetParent(achievmentsContent.transform, false);
                var achievmentContainer = go.GetComponent<AchievmentContainer>();
                achievmentContainer.Setup(achievments[i]);
                achievmentInst.Add(achievmentContainer);
            }
        }

        private void SetupCollections() {
            var collections = RewardManager.Instance.GetCollections();
            for (int i = 0; i < collections.Count; i++) {
                var go = GameObject.Instantiate<GameObject>(collectionPrefab);
                go.transform.SetParent(collectionsContent.transform, false);
                var collectionContainer = go.GetComponent<CollectionContainer>();
                collectionContainer.Setup(collections[i]);

                collectionsInst.Add(collectionContainer);
            }
        }

        private void RefreshDisplay() {
            switch (inset) {
                case Inset.ACHIEVMENT:
                    achievmentInst.ForEach(x => x.RefreshDisplay());
                    break;
                case Inset.COLLECTIONS:
                    collectionsInst.ForEach(x => x.RefreshDisplay());
                    break;
            }
        }
    }
}
