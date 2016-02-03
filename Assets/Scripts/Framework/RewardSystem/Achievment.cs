using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

namespace Framework {
    public class Achievment : MonoBehaviour {
        public const string textPath = "gameplay/achievements";

        public string id;
        public int totalCount;

        public Sprite iconNormal;
        public Sprite iconGlow;

        [HideInInspector]
        public int currentCount;
        public string titlePath { get; private set; }
        public string descriptionPath { get; private set; }

        public AchievmentInfo GetInfo() {
            return new AchievmentInfo {
                id = id,
                totalCount = totalCount,
                currentCount = currentCount,
                titlePath = "{" + textPath + "/" + id + "/title}",
                descriptionPath = "{" + textPath + "/" + id + "/description}",
                iconNormal = iconNormal,
                iconGlow = iconGlow

            };
        }

        public XmlNode Save(XmlDocument doc) {
            XmlElement achievment = doc.CreateElement(id);
            achievment.SetAttribute("current_count", Convert.ToString(currentCount));
            return achievment;
        }

        public override string ToString() {
            return "Achievment: id = " + id + ", totalCount = " + totalCount + ", currentCount = " + currentCount;
        }
    }

    public class AchievmentInfo {
        public string id;

        public int totalCount;
        public int currentCount;

        public string titlePath;
        public string descriptionPath;

        public Sprite iconNormal;
        public Sprite iconGlow;
    }
}
