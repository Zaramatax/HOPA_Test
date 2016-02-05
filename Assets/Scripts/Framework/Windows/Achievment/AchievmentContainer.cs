using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Framework {
    public class AchievmentContainer : MonoBehaviour {

        public Image icon;
        public Text title;
        public Text description;
        public Text counter;

        private Achievment achievment;

        public void Setup(Achievment achievment) {
            this.achievment = achievment;
            icon.sprite = achievment.IconNormal;
            title.text = LocalizationManager.GetTranslationStatic(achievment.Title);
            description.text = LocalizationManager.GetTranslationStatic(achievment.Description);
            counter.text = achievment.currentCount + "/" + achievment.TotalCount;
        }

        public void RefreshDisplay() {
            counter.text = achievment.currentCount + "/" + achievment.TotalCount;
            if (achievment.currentCount == achievment.TotalCount) {
                icon.sprite = achievment.IconGlow;
            }
        }
    }
}
