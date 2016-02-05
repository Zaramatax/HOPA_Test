using System;
using System.Xml;
using UnityEngine;

namespace Framework {
    public class Achievment : MonoBehaviour, IReward {
        public const string textPath = "gameplay/achievements";

        [SerializeField]
        private int totalCount;
        [SerializeField]
        private Sprite iconNormal;
        [SerializeField]
        private Sprite iconGlow;
        [SerializeField]
        private int score;

        public string id { get; private set; }
        public int currentCount { get; private set; }
        public int notGivenCount { get; private set; }

        public int      TotalCount      { get { return totalCount; } }
        public Sprite   IconNormal      { get { return iconNormal; } }
        public Sprite   IconGlow        { get { return iconGlow; } }
        public string   Title           { get { return "{" + textPath + "/" + id + "/title}"; } }
        public string   Description     { get { return "{" + textPath + "/" + id + "/description}"; }}

        public void Init() {
            id = gameObject.name;
            currentCount = 0;
            notGivenCount = 0;
        }

        public bool CheckNotGiven() {
            return notGivenCount > 0 ? true : false;
        }

        public void MarkAsNotGiven() {
            notGivenCount++;
            TryGiveReward();
        }

        public void TryGiveReward() {
            if (RewardManager.Instance.GetAchievmentBannerState() != AchievmentBannerState.DEFAULT) return;
            if(currentCount == totalCount) {
                notGivenCount = 0;
                return;
            }
            notGivenCount--;
            currentCount++;
            RewardManager.Instance.GiveReward(this);
        }

        public int GetScore() {
            return score;
        }

        public XmlNode Save(XmlDocument doc) {
            XmlElement achievment = doc.CreateElement(id);
            achievment.SetAttribute("current_count", Convert.ToString(currentCount));
            achievment.SetAttribute("not_given_count", Convert.ToString(notGivenCount));
            return achievment;
        }

        public void Load(XmlElement achievment) {
            if (achievment == null) return;

            var currentCountValue = achievment.GetAttribute("current_count");
            if (currentCountValue != null) {
                currentCount = Convert.ToInt32(currentCountValue);
            }

            var notGivenCountValue = achievment.GetAttribute("not_given_count");
            if (notGivenCountValue != null) {
                notGivenCount = Convert.ToInt32(notGivenCountValue);
            }
        }
    }
}
