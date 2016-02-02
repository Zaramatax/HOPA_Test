using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework {
    public class AchievmentBanner : MonoBehaviour {

        private State state;
        private Animator animator;
        private Achievment currentAchievment;

        private int moveState;
        private int currentState;

        private AchievmentManager achievmentManager;

        void Awake() {
            moveState = Animator.StringToHash("show_achievment_banner");
            animator = GetComponent<Animator>();
        }

        void Start() {
            achievmentManager = AchievmentManager.Instance;
            achievmentManager.NewAchievment += OnNewAchievment;
        }

        public void OnNewAchievment() {
            ShowBanner();
        }

        public void ShowBanner() {
            if (currentAchievment != null) return;
            currentAchievment = achievmentManager.GetAchievmentForBanner();

            if (currentAchievment == null) return;

            currentState = animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
            if (moveState == currentState) return;

            animator.Play(moveState);
        }

        public void OnBannerShowComplete() {
            currentAchievment = null;
            ShowBanner();
        }

    }
}
