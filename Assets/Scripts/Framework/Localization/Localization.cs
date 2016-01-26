using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Framework {
    public class Localization : MonoBehaviour {
        public static Dictionary<string, Localization> languages;

        public string resourcePath;

        [System.NonSerialized]
        public Dictionary<string, string> translations;

        [System.NonSerialized]
        public TextAsset writable;

        [System.NonSerialized]
        public bool hasChanges = false;

        static IEnumerable<XElement> SimpleStreamAxis (byte[] text,
                                                  params string[] elements) {
            using (XmlReader reader = XmlReader.Create(new MemoryStream(text), new XmlReaderSettings() { IgnoreWhitespace = true })) {
                reader.MoveToContent();
                while (reader.Read()) {
                    if (reader.NodeType == XmlNodeType.Element) {
                        //if (reader.GetAttribute("text") != null)
                        if (System.Array.IndexOf(elements, reader.Name) >= 0) {
                            XElement el = XNode.ReadFrom(reader) as XElement;
                            if (el != null) {
                                yield return el;
                            }
                        }
                    }
                }
            }
        }

        void xmlTreeSearch (XElement cur, string path, System.Action<XElement, string> OnElement) {
            foreach (var elem in cur.Elements()) {
                if (elem.HasElements)
                    xmlTreeSearch(elem, path + elem.Name.LocalName + "/", OnElement);
                else
                    OnElement(elem, path);
            }
        }

        void LoadXml (TextAsset text) {
            using (XmlReader reader = XmlReader.Create(new StreamReader(new MemoryStream(text.bytes), true), new XmlReaderSettings() { IgnoreWhitespace = true })) {
                reader.MoveToContent();
                XElement el = XNode.ReadFrom(reader) as XElement;
                xmlTreeSearch(el, "", (x, p) => {
                    if (x.Attribute("text") != null) {
                        var key = p + x.Name.LocalName;
                        if (!translations.ContainsKey(key))
                            translations.Add(key, x.Attribute("text").Value);
                        else
                            Debug.LogWarning("duplicate key: " + key);
                    }
                });
            }

            //foreach (var elem in SimpleStreamAxis(text.bytes, "inventory_items", "location_titles", "task_from_imp", "main_menu"))
            {

                /*if (!translations.ContainsKey(elem.Name.LocalName))
                    translations.Add(elem.Name.LocalName, elem.Attribute("text").Value);
                else
                    Debug.LogWarning("Duplicate translation key: " + elem.Name.LocalName);*/
                /*foreach (var sub in elem.Elements())
                {
                    if (sub.Attribute("text") != null)
                    {
                        if (!translations.ContainsKey(sub.Name.LocalName))
                            translations.Add(sub.Name.LocalName, sub.Attribute("text").Value);
                        else
                            Debug.LogWarning("Duplicate translation key: " + sub.Name.LocalName);
                    }
                }*/

                /*
                XmlReader reader = XmlReader.Create(new MemoryStream(text.bytes), new XmlReaderSettings() { IgnoreWhitespace = true });
                int level = 0;
                reader.MoveToContent();
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "inventory_items")
                        {
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.EndElement)
                                    break;

                                Debug.Log(reader.Name);
                            }
                            reader.Read();
                        }
                        else
                        {
                            if (!reader.IsEmptyElement)
                            {
                                level++;
                          //      parent = node.transform;
                            }
                        }
                    }
                    if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        level--;
                        //parent = parent.parent;
                    }
                }*/
            }
        }

        void LoadCSV (TextAsset text) {
            var sr = new StreamReader(new MemoryStream(text.bytes));
            string current_preffix = "";
            while (!sr.EndOfStream) {
                var line = sr.ReadLine();
                var key = line.Remove(line.IndexOf(','));
                var val = line.Substring(line.IndexOf(',') + 1);
                if (key.StartsWith("--preffix"))
                    current_preffix = val;
                else {
                    if (!translations.ContainsKey(current_preffix + key))
                        translations.Add(current_preffix + key, val);
                    else
                        Debug.LogWarning("Duplicate key: " + current_preffix + key);
                }
            }
            sr.Close();
        }

        public void Load () {
            translations = new Dictionary<string, string>();
            writable = null;
            hasChanges = false;
            foreach (TextAsset text in Resources.LoadAll<TextAsset>(resourcePath)) {
                if (text.bytes[0] == 0x003c)
                    LoadXml(text);
                else {
                    if (writable == null)
                        writable = text;
                    LoadCSV(text);
                }
            }
        }

        public string GetTranslation (string key) {
            if (translations == null)
                Load();

            if (key.StartsWith("{")) {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                int start = 0;
                while (start >= 0) {
                    int end = key.IndexOf('}', start);
                    if (end > start) {
                        string sub_key = key.Substring(start + 1, end - start - 1);
                        if (translations.ContainsKey(sub_key))
                            sb.Append(translations[sub_key]);
                        else
                            sb.Append("{" + sub_key + "}");
                    }
                    else {
                        sb.Append(key.Substring(start));
                        break;
                    }

                    start = key.IndexOf('{', end);
                    if (start > 0)
                        sb.Append(key.Substring(end + 1, start - end - 1));
                    else
                        sb.Append(key.Substring(end + 1));
                }
                return sb.ToString();
            }

            if (translations.ContainsKey(key))
                return translations[key];

            return key;
        }

        public void Save () {
#if UNITY_EDITOR
            StreamWriter sw = new StreamWriter(AssetDatabase.GetAssetPath(writable));
            foreach (var kvp in translations)
                sw.WriteLine(kvp.Key + "," + kvp.Value);
            sw.Close();
            hasChanges = false;
#endif
        }

        static string AUTO_PROPERTY_OPEN = "[";
        static string AUTO_PROPERTY_CLOSE = "]";

        public static string ParseKey (string key, MonoBehaviour context) {
            var keySrc = key;
            if (keySrc.Contains(AUTO_PROPERTY_OPEN) && keySrc.Contains(AUTO_PROPERTY_CLOSE) && context != null) {
                var from = keySrc.IndexOf(AUTO_PROPERTY_OPEN);
                var to = keySrc.IndexOf(AUTO_PROPERTY_CLOSE);
                var field = keySrc.Substring(from + 1, to - from - 1);
                var prop = context.GetType().GetField(field);
                if (prop != null) {
                    keySrc = keySrc.Remove(from) + prop.GetValue(context) + keySrc.Substring(to + 1);
                }
            }
            return keySrc;
        }
    }

    public class LocalizationStringAttribute : PropertyAttribute {
        public LocalizationStringAttribute () {
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(LocalizationStringAttribute))]
    public class ObjectEnumPropertyDrawer : PropertyDrawer {
        static Localization current = null;
        static Localization[] all = null;

        static MonoBehaviour currentUi;
        static UILocalizationGroup currentGroup;
        static List<string> vars = null;
        static bool reloadVars = false;

        public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
            return base.GetPropertyHeight(property, label) * (string.IsNullOrEmpty(property.stringValue) ? 1 : 2);
        }

        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
            if (all == null) {
                all = Resources.LoadAll<Localization>("Localization");
                var stored = EditorPrefs.GetString("locale");
                current = Array.Find(all, a => a.name == stored);
            }

            if (current == null && all.Length > 0)
                current = all[0];
            var keyInfo = attribute as LocalizationStringAttribute;

            var newUi = property.serializedObject.targetObject as MonoBehaviour;
            if (newUi != currentUi) {
                reloadVars = true;
                currentUi = newUi;
                currentGroup = UILocalizationGroup.Find(currentUi.gameObject);
            }
            
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Keyboard), label);

            if (current != null) {
                if (current.translations == null)
                    current.Load();

                if (reloadVars) {
                    vars = new List<string>(current.translations.Keys).FindAll(m => currentGroup == null || m.StartsWith(currentGroup.preffix)).ConvertAll(c => c.Substring(currentGroup != null ? currentGroup.preffix.Length + 1 : 0));
                    reloadVars = false;
                }

                // Don't make child fields be indented
                var indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                // Calculate rects
                var amountRect = new Rect(position.x, position.y, position.width - 80, position.height);
                var refreshRect = new Rect(position.x + position.width - 80, position.y, 60, base.GetPropertyHeight(property, label));
                var clearRect = new Rect(position.x + position.width - 20, position.y, 20, position.height);
                // Draw fields - passs GUIContent.none to each so they are drawn without labels
                //EditorGUI.PropertyField(amountRect, property, GUIContent.none);

                var amountRectHalf = new Rect(amountRect.x, amountRect.y, amountRect.width - (vars != null ? 20 : 0), amountRect.height / (string.IsNullOrEmpty(property.stringValue) ? 1 : 2));
                property.stringValue = EditorGUI.TextField(amountRectHalf, property.stringValue);

                if (vars != null) {
                    var newVar = EditorGUI.Popup(new Rect(amountRect.x + amountRect.width - 20, amountRect.y, 20, amountRect.height / (string.IsNullOrEmpty(property.stringValue) ? 1 : 2)), -1, vars.ToArray(), GUI.skin.button);
                    if (newVar >= 0) {
                        property.stringValue = "{" + (currentGroup != null ? currentGroup.preffix + "/" : "") + vars[newVar] + "}";
                    }
                }

                if (!string.IsNullOrEmpty(property.stringValue)) {
                    amountRect = new Rect(amountRect.x, amountRect.y + amountRect.height / 2, amountRect.width + 60, amountRect.height / 2);
                    var keySrc = Localization.ParseKey(property.stringValue, property.serializedObject.targetObject as MonoBehaviour);
                    string keyStr = (currentGroup != null ? currentGroup.preffix + "/" : "") + keySrc;

                    bool editable = current.writable != null;
                    var start = keyStr.IndexOf('{');
                    var end = keyStr.IndexOf('}');
                    if (start >= 0 && end > start) {
                        string sub_key = keyStr.Substring(start + 1, end - start - 1);
                        if (current.translations.ContainsKey(sub_key)) {
                            if (editable) {
                                var newStr = EditorGUI.TextField(new Rect(amountRect.x, amountRect.y, amountRect.width - (current.hasChanges ? 60 : 0), amountRect.height), current.translations[keyStr]);
                                if (newStr != current.translations[sub_key]) {
                                    current.translations[sub_key] = newStr;
                                    current.hasChanges = true;
                                }

                                if (current.hasChanges && GUI.Button(new Rect(amountRect.x + amountRect.width - 60, amountRect.y, 60, amountRect.height), "save")) {
                                    current.Save();
                                }
                            }
                            else
                                EditorGUI.LabelField(amountRect, current.translations[sub_key]);
                        }
                        else {
                            EditorGUI.LabelField(new Rect(amountRect.x, amountRect.y, amountRect.width - (editable ? 60 : 0), amountRect.height), "key '" + sub_key + "' not found in dictionary");
                            if (editable && GUI.Button(new Rect(amountRect.x + amountRect.width - 60, amountRect.y, 60, amountRect.height), "create")) {
                                current.translations.Add(sub_key, "");
                                current.hasChanges = true;
                            }
                        }
                    }
                }

                int prev = Array.IndexOf(all, current);
                int cur = EditorGUI.Popup(refreshRect, prev, Array.ConvertAll(all, c => c.name));
                if (cur != prev) {
                    current = all[cur];
                    EditorPrefs.SetString("locale", current != null ? current.name : "");
                    reloadVars = true;
                }

                if (GUI.Button(clearRect, "R")) {
                    current.translations.Clear();
                    current.translations = null;
                }

                // Set indent back to what it was
                EditorGUI.indentLevel = indent;

                EditorGUI.EndProperty();
            }
        }
    }

#endif
}