using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Framework {
    public class AchievmentBanner : MonoBehaviour {

        private const string moveAnimName = "show_achievment_banner";

        private Animator animator;
        private AchievmentInfo currentAchievInfo;

        private int moveState;
        private int currentState;

        private Text title;
        private Text description;
        private Text counter;
        private Image icon;

        private RewardManager rewardManager;

        void Awake() {
            moveState = Animator.StringToHash(moveAnimName);
            animator = GetComponent<Animator>();

            title = transform.FindChild("title").GetComponent<Text>();
            description = transform.FindChild("description").GetComponent<Text>();
            counter = transform.FindChild("counter").GetComponent<Text>();
            icon = transform.FindChild("icon").GetComponent<Image>();
        }

        void Start() {
            rewardManager = RewardManager.Instance;
            rewardManager.NewAchievment += OnNewAchievment;
        }

        public void OnNewAchievment() {
            ShowBanner();
        }

        public void OnBannerShowComplete() {
            currentAchievInfo = null;
            ShowBanner();
        }

        private void ShowBanner() {
            if (currentAchievInfo != null) return;
            currentAchievInfo = rewardManager.GetAchievmentInfo();

            if (currentAchievInfo == null) return;

            currentState = animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
            if (moveState == currentState) return;

            SetBannerInfo();
            animator.Play(moveState);
        }

        private void SetBannerInfo() {
            title.text = LocalizationManager.GetTranslationStatic(currentAchievInfo.titlePath);
            description.text = LocalizationManager.GetTranslationStatic(currentAchievInfo.descriptionPath);

            if (currentAchievInfo.totalCount == 1) {
                counter.text = "";
            } else {
                counter.text = currentAchievInfo.currentCount + "/" + currentAchievInfo.totalCount;
            }

            if(currentAchievInfo.currentCount == currentAchievInfo.totalCount) {
                icon.sprite = currentAchievInfo.iconGlow;
            } else {
                icon.sprite = currentAchievInfo.iconNormal;
            }
        }

    }
}
