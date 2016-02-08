using System;
using UnityEngine;
using UnityEngine.UI;

namespace Framework {
    public enum AchievmentBannerState { DEFAULT, MOVE }

    public class AchievmentBanner : MonoBehaviour {

        private const string moveAnimName = "show_achievment_banner";

        private Animator animator;
        private Achievment currentAchievment;

        private int moveState;
        private int currentState;

        public Text title;
        public Text description;
        public Text counter;
        public Image icon;

        private RewardManager rewardManager;

        public event Action MoveComplete;

        public AchievmentBannerState State {
            get {
                currentState = animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
                if (moveState == currentState) {
                    return AchievmentBannerState.MOVE;
                } else {
                    return AchievmentBannerState.DEFAULT;
                }
            }
        }

        void Awake() {
            moveState = Animator.StringToHash(moveAnimName);
            animator = GetComponent<Animator>();
        }

        void Start() {
            rewardManager = RewardManager.Instance;
            rewardManager.NewReward += OnNewReward;
            rewardManager.AddAchievmentBanner(this);
        }

        void OnDestroy() {
            rewardManager.NewReward -= OnNewReward;
        }

        public void OnNewReward(object reward, EventArgs e) {
            currentAchievment = reward as Achievment;
            if(currentAchievment != null)
                ShowBanner();
        }

        public void OnBannerShowComplete() {
            rewardManager.OnAchievmentBannerMoveComplete();
        }

        private void ShowBanner() {
            SetBannerInfo();
            animator.Play(moveState);
        }

        private void SetBannerInfo() {
            title.text = LocalizationManager.GetTranslationStatic(currentAchievment.Title);
            description.text = LocalizationManager.GetTranslationStatic(currentAchievment.Description);

            if (currentAchievment.TotalCount == 1) {
                counter.text = "";
            } else {
                counter.text = currentAchievment.currentCount + "/" + currentAchievment.TotalCount;
            }

            if(currentAchievment.currentCount == currentAchievment.TotalCount) {
                icon.sprite = currentAchievment.IconGlow;
            } else {
                icon.sprite = currentAchievment.IconNormal;
            }
        }
    }
}
