using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

namespace Framework{

    public class AchievmentManager : MonoBehaviour {

        public string configPath;

        public static AchievmentManager Instance;

        public event Action NewAchievment;

        private List<Achievment> achievments;
        private List<Achievment> achievmentsForBanner;

        void Awake() {
            if(Instance != null) {
                DestroyImmediate(gameObject);
            } else {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }

#if UNITY_EDITOR
            var root = Utils.GetDocRootNode(configPath);
#else
            var root = Utils.GetDocRootNode(Application.dataPath + "/../" + configPath);
#endif
            var achievmentsConfig = Utils.GetElement(root, "achievments");

            achievments = new List<Achievment>();
            achievmentsForBanner = new List<Achievment>();

            foreach (XmlNode achievmentConfig in achievmentsConfig) {
                var id = Utils.GetAttribute(achievmentConfig, "id");
                var totalCount = Utils.GetAttribute(achievmentConfig, "total_count");

                if (id == null || totalCount == null) continue;

                AddAchievment(id, Convert.ToInt32(totalCount));
            }

            // тут надо загрузить инфу о количестве полученных ачивок
        }

        void Update() {
            if (Input.GetMouseButtonUp(0)) {
                GiveAchievment("ho_master");
                GiveAchievment("ho_master");
            }
        }

        private void AddAchievment(string id, int totalCount, int currentCount = 0) {
            achievments.Add(new Achievment(id, totalCount, currentCount));
        }

        public Achievment GetAchievmentForBanner() {
            if (achievmentsForBanner.Count == 0) return null;

            var achiev = achievmentsForBanner.Last();
            achievmentsForBanner.Remove(achiev);
            return achiev;
        }

        public void GiveAchievment(string id) {
            var achievment = achievments.Find(x => x.id == id);

            achievment.currentCount++;
            achievmentsForBanner.Add(new Achievment(achievment.id, achievment.totalCount, achievment.currentCount));

            if (NewAchievment != null)
                NewAchievment();
        }
    }

    public class Achievment {
        public string id { get; private set; }
        public int totalCount { get; private set; }
        public int currentCount;

        public Achievment(string id, int totalCount, int currentCount = 0) {
            this.id = id;
            this.totalCount = totalCount;
            this.currentCount = currentCount;
        }

        public override string ToString() {
            return "Achievment: id = " + id + ", completeCount = " + totalCount + ", currentCount = " + currentCount;
        }
    }
}
