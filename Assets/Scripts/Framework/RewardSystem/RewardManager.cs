using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

namespace Framework{
    public class RewardManager : MonoBehaviour {
        public const string saveFileName = "reward_system";

        public static RewardManager Instance;

        public event Action NewAchievment;

        private int scorePoints;
        private List<Achievment> achievments;
        private Queue<AchievmentInfo> achievmentsForBanner;
        private List<Collection> collections;

        void Awake() {
            if (Instance != null) {
                DestroyImmediate(gameObject);
            } else {
                Instance = this;
            }

            achievments = new List<Achievment>();
            achievmentsForBanner = new Queue<AchievmentInfo>();
            collections = new List<Collection>();

            SetupAchievments();
            SetupCollections();
            scorePoints = 0;

            Load();
        }

        void Update() {
            if (Input.GetMouseButtonUp(0)) {
                GiveAchievment("curious");
                Save();
            }
        }

        void OnDestroy() {
            Save();
        }

        public AchievmentInfo GetAchievmentInfo() {
            if (achievmentsForBanner.Count == 0) return null;
            return achievmentsForBanner.Dequeue();
        }

        public void GiveAchievment(string id) {
            var achievment = achievments.Find(x => x.id == id);
            if (achievment.currentCount == achievment.totalCount) return;

            achievment.currentCount++;
            achievmentsForBanner.Enqueue(achievment.GetInfo());

            if (NewAchievment != null)
                NewAchievment();
        }

        public void AddScorePoints(int value) {
            scorePoints += value;
        }

        public CollectionItem GetCollectionItem(string collectionId, string itemId) {
            var collection = collections.Find(col => col.id == collectionId);
            if (collection == null) throw new Exception("Unknown collection ID '" + collectionId + "'.");

            var item = collection.items.Find(it => it.id == itemId);
            if (item == null) throw new Exception("Unknown collection item ID '" + itemId + "'.");

            return item;
        }

        public void CollectItem(string collectionId, string itemId) {
            var item = GetCollectionItem(collectionId, itemId);
            item.collected = true;
            AddScorePoints(item.score);
        }

        public bool IsCollected(string collectionId, string itemId) {
            var item = GetCollectionItem(collectionId, itemId);
            return item.collected;
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

            XmlNode achievmentsForBannerNode = doc.CreateElement("achievments_for_banner");
            foreach(AchievmentInfo info in achievmentsForBanner) {
                XmlElement achievmentInfo = doc.CreateElement(info.id);
                achievmentInfo.SetAttribute("count", Convert.ToString(info.currentCount));
                achievmentsForBannerNode.AppendChild(achievmentInfo);
            }
            root.AppendChild(achievmentsForBannerNode);

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
            if(scoreNode != null) {
                var scorePointsValue = scoreNode.GetAttribute("value");
                if (scorePointsValue != null)
                    scorePoints = Convert.ToInt32(scorePointsValue);
            }

            XmlElement achievmentsNode = Utils.GetElement(root, "achievments");
            if (achievmentsNode != null) {
                foreach (XmlElement achievmentInfo in achievmentsNode) {
                    var currentAchievment = achievments.Find(x => x.id == achievmentInfo.Name);
                    if (currentAchievment == null) continue;

                    currentAchievment.Load(achievmentInfo);
                }
            }

            XmlElement achievmentsForBannerNode = Utils.GetElement(root, "achievments_for_banner");
            if(achievmentsForBannerNode != null) {
                foreach (XmlElement achievmentsForBannerInfo in achievmentsForBannerNode) {
                    var currentAchievment = achievments.Find(x => x.id == achievmentsForBannerInfo.Name);
                    if (currentAchievment == null) continue;

                    var countValue = achievmentsForBannerInfo.GetAttribute("count");
                    if (countValue == null) continue;

                    var currentAchievmentInfo = currentAchievment.GetInfo();
                    currentAchievmentInfo.currentCount = Convert.ToInt32(countValue);
                    achievmentsForBanner.Enqueue(currentAchievmentInfo);
                }
            }

            XmlNode collectionsNode = Utils.GetElement(root, "collections");
            if (collectionsNode != null) {
                foreach (XmlElement collectionInfo in collectionsNode) {
                    var currentCollection = collections.Find(x => x.id == collectionInfo.Name);
                    if (currentCollection == null) continue;

                    currentCollection.Load(collectionInfo);
                }
            }
        }

        private void SetupAchievments() {
            achievments = Resources.LoadAll<Achievment>("Prefabs/Achievments").ToList();
            achievments.ForEach(x => x.Init());
        }

        private void SetupCollections() {
            collections = Resources.LoadAll<Collection>("Prefabs/Collections").ToList();
            collections.ForEach(col => col.Init());
        }
    }
}
