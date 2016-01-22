using System.IO;
using System.Xml;
using UnityEngine;

namespace Framework {
    public static class ProfileSaver {
        private static string profilePath = "profile/";

        public static string ProfilePath {
            get {
                if (!Directory.Exists(profilePath)) {
                    FileInfo profileFolder = new FileInfo(profilePath);
                    profileFolder.Directory.Create();
                }

                return profilePath;
            }
        }

        public static void Save (XmlDocument doc, string name) {
            doc.Save(ProfilePath + name + ".xml");
        }

        public static bool Load (XmlDocument doc, string name) {
            try {
                doc.Load(ProfilePath + name + ".xml");
                return true;
            }
            catch { return false; };
        }
    }
}