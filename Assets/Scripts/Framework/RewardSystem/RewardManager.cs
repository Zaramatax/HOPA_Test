using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
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

        public void CollectItem(string collectionId, string itemId) {
            var collection = collections.Find(col => col.id == collectionId);
            if (collection == null) throw new Exception("Unknown collection ID '" + collectionId + "'.");

            var item = collection.items.Find(it => it.id == itemId);
            if(item == null) throw new Exception("Unknown collection item ID '" + itemId + "'.");

            item.collected = true;
        }

        public bool IsCollected(string collectionId, string itemId) {
            var collection = collections.Find(col => col.id == collectionId);
            if (collection == null) throw new Exception("Unknown collection ID '" + collectionId + "'.");

            var item = collection.items.Find(it => it.id == itemId);
            if (item == null) throw new Exception("Unknown collection item ID '" + itemId + "'.");

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
                scorePoints = Convert.ToInt32(scoreNode.GetAttribute("value"));
            }

            XmlNode achievmentsNode = Utils.GetElement(root, "achievments");
            if (achievmentsNode != null) {
                foreach (XmlElement achievElement in achievmentsNode) {
                    var currentAchievment = achievments.Find(x => x.id == achievElement.Name);
                    if (currentAchievment == null) continue;

                    currentAchievment.currentCount = Convert.ToInt32(achievElement.GetAttribute("current_count"));
                }
            }

            XmlNode collectionsNode = Utils.GetElement(root, "collections");
            if (collectionsNode != null) {
                foreach (XmlElement collectionElement in collectionsNode) {
                    var currentCollection = collections.Find(x => x.id == collectionElement.Name);
                    if (currentCollection == null) continue;

                    foreach (XmlElement itemElement in collectionElement) {
                        var currentItem = currentCollection.items.Find(x => x.id == itemElement.Name);
                        if (currentItem == null) continue;

                        currentItem.collected = Convert.ToBoolean(itemElement.GetAttribute("collected"));
                    }
                }
            }
        }

        private void SetupAchievments() {
            achievments = Resources.LoadAll<Achievment>("Prefabs/Achievments").ToList();
            achievments.ForEach(x => x.currentCount = 0);
        }

        private void SetupCollections() {
            collections = Resources.LoadAll<Collection>("Prefabs/Collections").ToList();
            collections.ForEach(col => col.items.ForEach(it => it.collected = false));
        }
    }
}
