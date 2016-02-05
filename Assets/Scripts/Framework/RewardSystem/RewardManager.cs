using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

namespace Framework{
    public class RewardManager : MonoBehaviour {
        private const string saveFileName = "reward_system";

        public static RewardManager Instance;

        public event EventHandler NewReward;

        private AchievmentBanner achievmentBanner;

        public int scorePoints { get; private set; }
        private List<Achievment> achievments;
        private List<Collection> collections;

        public int CollectionsCount { get { return collections.Count; } }
        public int AchievmentsCount { get { return achievments.Count; } }

        void Awake() {
            if (Instance != null) {
                DestroyImmediate(gameObject);
            } else {
                Instance = this;
            }

            achievments = new List<Achievment>();
            collections = new List<Collection>();

            SetupAchievments();
            SetupCollections();
            scorePoints = 0;

            Load();
        }

        void Start() {
            
        }

        void Update() {
            if (Input.GetMouseButtonUp(0)) {
                GiveAchievment("curious");
            }
        }

        void OnDestroy() {
            Save();
        }

        public void AddAchievmentBanner(AchievmentBanner banner) {
            achievmentBanner = banner;
            achievmentBanner.MoveComplete += CheckNotGivenAchievmentReward;
        }

        public void CheckNotGivenAchievmentReward() {
            foreach (Achievment achievment in achievments) {
                if (achievment.CheckNotGiven()) {
                    achievment.TryGiveReward();
                }
            }
        }

        public AchievmentBannerState GetAchievmentBannerState() {
            return achievmentBanner.State;
        }


        public void GiveReward(IReward reward) {
            if (reward.GetScore() != 0)
                AddScorePoints(reward.GetScore());

            if (NewReward != null)
                NewReward(reward, EventArgs.Empty);
        }

        public void GiveAchievment(string id) {
            var achievment = GetAchievment(id);
            if (achievment.currentCount == achievment.TotalCount) return;
            achievment.MarkAsNotGiven();
        }

        public void CollectItem(string collectionId, int itemIndex) {
            GetCollection(collectionId).GetItem(itemIndex).MarkAsCollected();
        }

        private void AddScorePoints(int value) {
            scorePoints += value;
        }

        public Achievment GetAchievment(string id) {
            var achievment = achievments.Find(x => x.id == id);
            if (achievment == null) throw new Exception("Unknown achievment ID '" + id + "'");
            return achievment;
        }

        public Achievment GetAchievment(int index) {
            try {
                return achievments[index];
            } catch {
                throw new Exception("Achievment Index is out of range: index = '" + index + "'.");
            }
        }

        public Collection GetCollection(string id) {
            var collection = collections.Find(col => col.id == id);
            if (collection == null) throw new Exception("Unknown collection ID '" + id + "'.");
            return collection;
        }

        public Collection GetCollection(int index) {
            try {
                return collections[index];
            } catch {
                throw new Exception("Collection Index is out of range: index = '" + index + "'.");
            }
        }

        public void Save() {
            XmlDocument doc = new XmlDocument();
            XmlNode root = doc.CreateElement("reward_system");
            doc.AppendChild(root);

            XmlElement scoreNode = doc.CreateElement("score_points");
            scoreNode.SetAttribute("value", Convert.ToString(scorePoints));
            root.AppendChild(scoreNode);

            XmlNode achievmentsNode = doc.CreateElement("achievments");
            achievments.ForEach(x => achievmentsNode.AppendChild(x.Save(doc)));
            root.AppendChild(achievmentsNode);

            XmlNode collectionsNode = doc.CreateElement("collections");
            collections.ForEach(x => collectionsNode.AppendChild(x.Save(doc)));
            root.AppendChild(collectionsNode);

            ProfileSaver.Save(doc, saveFileName);
        }

        public void Load() {
            XmlDocument doc = new XmlDocument();
            ProfileSaver.Load(doc, saveFileName);
            XmlElement root = doc.DocumentElement;
            if (root == null) return;

            XmlElement scoreNode = Utils.GetElement(root, "score_points");
            if (scoreNode != null) {
                var scorePointsValue = scoreNode.GetAttribute("value");
                if (scorePointsValue != null)
                    scorePoints = Convert.ToInt32(scorePointsValue);
            }

            XmlElement achievmentsNode = Utils.GetElement(root, "achievments");
            if (achievmentsNode != null) {
                foreach (XmlElement achievment in achievmentsNode) {
                    var currentAchievment = achievments.Find(x => x.id == achievment.Name);
                    if (currentAchievment == null) continue;
                    currentAchievment.Load(achievment);
                }
            }

            XmlNode collectionsNode = Utils.GetElement(root, "collections");
            if (collectionsNode != null) {
                foreach (XmlElement collection in collectionsNode) {
                    var currentCollection = collections.Find(x => x.id == collection.Name);
                    if (currentCollection == null) continue;
                    currentCollection.Load(collection);
                }
            }
        }

        private void SetupAchievments() {
            achievments = Resources.LoadAll<Achievment>("Prefabs/Achievments").ToList();
            achievments.ForEach(x => x.Init());
        }

        private void SetupCollections() {
            collections = Resources.LoadAll<Collection>("Prefabs/Collections").ToList();
            collections.ForEach(x => x.Init());
        }
    }
}
