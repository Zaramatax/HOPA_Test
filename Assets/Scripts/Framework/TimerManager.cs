using System.Xml;
using System.Collections.Generic;

public class TimerManager {

	private const string timersNodeName = "timers";

	private List<Timer> timers;

	public TimerManager() {
		timers = new List<Timer> ();
	}

	public void Save(XmlDocument doc) {
		XmlNode timersNode = doc.CreateElement(timersNodeName);
		doc.DocumentElement.AppendChild(timersNode);

		for (int i = 0; i < timers.Count; i++)
			timers[i].SaveToXML (timersNode, doc);
	}

	public void Load(XmlDocument doc) {
		XmlNode timersNode = doc.DocumentElement.SelectSingleNode(timersNodeName);

		for (int i = 0; i < timers.Count; i++)
			timers[i].LoadFromXML (timersNode);
	}

	public void UpdateTimers(float delta) {
		for (int i = 0; i < timers.Count; i++)
			timers[i].Update (delta);
	}

	public Timer CreateTimer(string name, float time, System.Action action) {
		timers.Add (new Timer (name, time, action));

		return timers [timers.Count - 1];
	}

	public void StartTimer(string name) {
		for (int i = 0; i < timers.Count; i++) {
			if (timers [i].ToString () == name) {
				timers [i].Start ();
				return;
			}
		}
	}
}
