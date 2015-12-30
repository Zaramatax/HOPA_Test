using System.Xml;

public class Timer {
    private const string timeAttrName  = "time";
    private const string startAttrName = "start";

    private string name;

    private float time;

    private bool isStart;

    private System.Action action;

    public Timer(string name, float time, System.Action action) {
        this.name = name;
        this.time = time;
        this.action = action;
    }

    public void Start() {
        if (time <= 0f) return;

        isStart = true;
    }

    public void Update(float delta) {
        if (!isStart) return;

        time -= delta;

        if (time <= 0f) {
            isStart = false;
            action();
        }
    }

	public override string ToString () {
		return name;
	}

    public void LoadFromXML(XmlNode parent) {
        XmlNode node = parent.SelectSingleNode(name);

        if (node == null) return;

        isStart = node.Attributes[startAttrName].Value == "1";

        float.TryParse(node.Attributes[timeAttrName].Value, out time);
    }

    public void SaveToXML(XmlNode parent, XmlDocument doc) {
        XmlNode node = doc.CreateElement(name);

        XmlAttribute startAttr = doc.CreateAttribute(startAttrName);
        startAttr.Value = isStart ? "1" : "0";

        XmlAttribute timeAttr = doc.CreateAttribute(timeAttrName);
        timeAttr.Value = time.ToString();

        node.Attributes.Append(startAttr);
        node.Attributes.Append(timeAttr);

        parent.AppendChild(node);
    }
}